using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				// Fill Partners data
				this.partnersTableAdapter.ClearBeforeFill = true;
				this.partnersTableAdapter.Fill(this.mazikov_DEMO_bDataSet.Partners);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonPrev_Click(object sender, EventArgs e)
		{
			if (partnersBindingSource.Position > 0)
			{
				partnersBindingSource.Position -= 1;
			}
		}

		private void buttonNext_Click(object sender, EventArgs e)
		{
			if (partnersBindingSource.Position < partnersBindingSource.Count - 1)
			{
				partnersBindingSource.Position += 1;
			}
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			try
			{
				var table = this.mazikov_DEMO_bDataSet.Partners;
				var newRow = table.NewPartnersRow();
				if (!ValidatePartnerInputs(out string errorMessage))
				{
					MessageBox.Show(errorMessage, "Проверка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				newRow.PartnerType = textBoxPartnerType.Text.Trim();
				newRow.PartnerName = textBoxPartnerName.Text.Trim();
				newRow.Director = textBoxDirector.Text.Trim();
				newRow.Email = textBoxEmail.Text.Trim();
				newRow.Phone = textBoxPhone.Text.Trim();
				newRow.LegalAddress = textBoxLegalAddress.Text.Trim();
				newRow.INN = textBoxINN.Text.Trim();
				int ratingParsed;
				if (int.TryParse(textBoxRating.Text.Trim(), out ratingParsed))
				{
					newRow.Rating = ratingParsed;
				}
				else
				{
					newRow.SetRatingNull();
				}
				table.Rows.Add(newRow);
				SaveChanges();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Не удалось добавить запись: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonEdit_Click(object sender, EventArgs e)
		{
			try
			{
				if (!ValidatePartnerInputs(out string errorMessage))
				{
					MessageBox.Show(errorMessage, "Проверка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				partnersBindingSource.EndEdit();
				SaveChanges();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Не удалось сохранить изменения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonDelete_Click(object sender, EventArgs e)
		{
			try
			{
				if (partnersBindingSource.Current is DataRowView rowView)
				{
					if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						rowView.Row.Delete();
						SaveChanges();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Не удалось удалить запись: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonSave_Click(object sender, EventArgs e)
		{
			try
			{
				SaveChanges();
				MessageBox.Show("Изменения сохранены.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Не удалось сохранить изменения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SaveChanges()
		{
			Validate();
			partnersBindingSource.EndEdit();
			tableAdapterManager.UpdateAll(this.mazikov_DEMO_bDataSet);
		}

		private bool ValidatePartnerInputs(out string error)
		{
			error = string.Empty;
			string partnerType = textBoxPartnerType.Text == null ? string.Empty : textBoxPartnerType.Text.Trim();
			string partnerName = textBoxPartnerName.Text == null ? string.Empty : textBoxPartnerName.Text.Trim();
			if (string.IsNullOrEmpty(partnerType))
			{
				error = "Поле 'Тип партнёра' обязательно.";
				return false;
			}
			if (string.IsNullOrEmpty(partnerName))
			{
				error = "Поле 'Название' обязательно.";
				return false;
			}
			if (partnerType.Length > 10)
			{
				error = "'Тип партнёра' не должен превышать 10 символов.";
				return false;
			}
			if (partnerName.Length > 200)
			{
				error = "'Название' не должен превышать 200 символов.";
				return false;
			}
			return true;
		}
	}
}


