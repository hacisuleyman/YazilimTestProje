﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DemirbasOtomasyon.Controller;
using DemirbasOtomasyon.Model;

namespace DemirbasOtomasyon.View
{
    public partial class FormPersonelGuncelleme : DevExpress.XtraEditors.XtraForm
    {
        public FormPersonelGuncelleme()
        {
            InitializeComponent();
        }
        private void PersonelRenklendir()
        {
            try
            {
                for (int i = 0; i < dgwPersonelListesi.Rows.Count; i++)
                {
                    Application.DoEvents();
                    DataGridViewCellStyle rowColor = new DataGridViewCellStyle();
                    if (Convert.ToInt32(dgwPersonelListesi.Rows[i].Cells[0].Value) >= 1)
                    {
                        rowColor.BackColor = Color.LightSkyBlue;
                        rowColor.ForeColor = Color.Black;
                    }                 
                    dgwPersonelListesi.Rows[i].DefaultCellStyle = rowColor;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex);
            }
        }
        private void PersonelListele()
        {
            DemirbasTakipEntitiess db = new DemirbasTakipEntitiess();
            var data = db.sp_PersonelListele2();
            dgwPersonelListesi.DataSource = data.ToList();
            PersonelRenklendir();
        }

        private void FormPersonelGuncelleme_Load(object sender, EventArgs e)
        {
            this.Location = new Point(50, 100);
            Araclar.ComboBoxDepartmanGetir(cmbDepartmanGuncelle);
            cmbDepartmanGuncelle.SelectedItem = null;
            cmbDepartmanGuncelle.SelectedText = "Departman Seçiniz...";
            PersonelListele();
        }

        private void BtnPersonelGuncelle_Click(object sender, EventArgs e)
        {

            PersonelGuncelle();
        }
        private void PersonelGuncelle()
        {
            try
            {
                DateTime selectedDate = Convert.ToDateTime(calendarControl.SelectionStart.ToShortDateString());
                if (string.IsNullOrEmpty(txtAdi.Text) || string.IsNullOrEmpty(txtSoyadi.Text) || string.IsNullOrEmpty(txtTelefon.Text))
                {
                    MessageBox.Show("Personel Bilgileri Boş Geçilemez !");
                }
                if (selectedDate > DateTime.Now)
                {
                    MessageBox.Show("İşe giriş  tarihi bugünden daha sonraki bir tarih olamaz!");
                }

                Personeller personel = new Personeller
                {
                    personelID = int.Parse(txtPersonelID.Text),
                    adi = txtAdi.Text,
                    soyadi = txtSoyadi.Text,
                    cepTelefonu = txtTelefon.Text,
                    departmanID = Convert.ToInt32(cmbDepartmanGuncelle.SelectedValue),
                    iseGirisTarihi = selectedDate                   
                };
                PersonelController.PersonelGuncelle(personel);
                MessageBox.Show("Personel Başarıyla Güncellendi !", "İşlem Başarılı !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PersonelListele();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Meydana Geldi !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DgwPersonelListele_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txtPersonelID.Text = dgwPersonelListesi.CurrentRow.Cells[0].Value.ToString();
            txtAdi.Text = dgwPersonelListesi.CurrentRow.Cells[1].Value.ToString();
            txtSoyadi.Text = dgwPersonelListesi.CurrentRow.Cells[2].Value.ToString();
            txtTelefon.Text = dgwPersonelListesi.CurrentRow.Cells[3].Value.ToString();
            cmbDepartmanGuncelle.Text = dgwPersonelListesi.CurrentRow.Cells[4].Value.ToString();
            calendarControl.EditValue = dgwPersonelListesi.CurrentRow.Cells[5].Value;
        }
        
        private void BtnPersonelSil_Click(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show(txtAdi.Text + " İsimli Personel Silinsin Mi ?", "Karar ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sonuc == DialogResult.Yes)
            {
                PersonelController.PersonelSil(int.Parse(txtPersonelID.Text));
                MessageBox.Show(txtAdi.Text +" "+ txtSoyadi.Text + " İsimli Personel Silindi !", "İşlem Başarılı !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PersonelListele();
            }
            List<Zimmetler> zimmetler;
            zimmetler=ZimmetController.ZimmetListele();
            foreach (Zimmetler z in zimmetler)
            {
                if(z.personelID==int.Parse(txtPersonelID.Text))
                {
                    AtikController.AtikEkle(z.zimmetID);
                }
            }
        }

        private void txtAdi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
                 && !char.IsSeparator(e.KeyChar);
        }

        private void txtTelefon_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtSoyadi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
                 && !char.IsSeparator(e.KeyChar);
        }
    }
}