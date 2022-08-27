using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AtomosZ.DragonAid.Libraries
{
	public partial class NumberBox : UserControl
	{
		public delegate void OnValueChanged(NumberBox sender, decimal numericalValue);
		public event OnValueChanged onValueChanged;

		private string prefix = "";
		private string postfix = "";
		/// <summary>
		/// The number without prefix or postfix.
		/// </summary>
		private decimal value;
		private decimal previousValue;
		private bool hexadecimal;
		private decimal increment = 1;
		private decimal maximum = 100;
		private decimal minimum = 0;

		public NumberBox()
		{
			InitializeComponent();

			textBox.GotFocus += TextBox_GotFocus;
			textBox.LostFocus += TextBox_LostFocus;
		}

		private void TextBox_SizeChanged(object sender, EventArgs e)
		{
			textBox.Size = new Size(this.Size.Width - 3, 0);
			this.Size = new Size(this.Size.Width, textBox.Height + 3);
		}

		public decimal Increment
		{
			get { return increment; }
			set { increment = value; }
		}

		public decimal Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
				RefreshTextBox();
			}
		}
		public bool Hexadecimal
		{
			get { return hexadecimal; }
			set { hexadecimal = value; }
		}

		public decimal Maximum
		{
			get { return maximum; }
			set { maximum = value; }
		}

		public decimal Minimum
		{
			get { return minimum; }
			set { minimum = value; }
		}

		/// <summary>
		/// WARNING: Do not use!
		/// </summary>
		[Obsolete("WARNING: Do NOT use this!")]
		public new string Text
		{
			get { throw new Exception("I tried to warn you."); }
			set { throw new Exception("I tried to warn you."); }
		}

		public string Prefix
		{
			get { return prefix; }
			set
			{
				prefix = value;
				RefreshTextBox();
			}
		}

		public string Postfix
		{
			get { return postfix; }
			set
			{
				postfix = value;
				RefreshTextBox();
			}
		}

		public Color NumberBoxBackColor
		{
			get { return textBox.BackColor; }
			set { textBox.BackColor = value; }
		}

		public HorizontalAlignment TextAlign
		{
			get { return textBox.TextAlign; }
			set { textBox.TextAlign = value; }
		}

		public bool ReadOnly
		{
			get { return textBox.ReadOnly; }
			set { textBox.ReadOnly = value; }
		}

		private void RefreshTextBox()
		{
			if (value > maximum)
				value = maximum;
			else if (value < minimum)
				value = minimum;

			if (hexadecimal)
			{
				string text;
				int v = (int)value;
				if (maximum <= 0xFF)
					text = v.ToString("X2");
				else if (maximum <= 0xFFFF)
					text = v.ToString("X4");
				else
					text = v.ToString("X");
				textBox.Text = prefix + text + postfix;

			}
			else
				textBox.Text = prefix + value.ToString() + postfix;

			if (onValueChanged != null && value != previousValue)
				onValueChanged(this, value);
		}


		private void NumberOnly_TextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (hexadecimal && Uri.IsHexDigit(e.KeyChar))
				return;
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.')
				e.Handled = true;
		}

		private void NumberOnly_TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			var textbox = (sender as TextBox);
			var key = (char)e.KeyValue;
			if (e.KeyCode == Keys.Escape)
			{
				e.Handled = true;
				e.SuppressKeyPress = true;

				value = previousValue;
				RefreshTextBox();
				TextBox_LostFocus(sender, e);
			}
			else if (e.KeyCode == Keys.Enter)
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
				TextBox_LostFocus(sender, e);
			}
			else if (e.KeyCode == Keys.Subtract)
			{
				value -= increment;
				if (value < minimum)
					value = maximum;
				RefreshTextBox();
			}
			else if (e.KeyCode == Keys.Add)
			{
				value += increment;
				if (value > maximum)
					value = minimum;
				RefreshTextBox();
			}
			else if ((e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Decimal))
			{
				if (textbox.SelectionLength == textbox.Text.Length)
					textbox.Text = "";

				if (textbox.Text.IndexOf('.') > -1)
				{ // only allow one decimal point
					e.Handled = true;
					e.SuppressKeyPress = true;

					return;
				}
			}
		}


		private void TextBox_GotFocus(object sender, EventArgs e)
		{
			previousValue = value;
		}

		private void TextBox_LostFocus(object sender, EventArgs e)
		{
			var val = textBox.Text;
			if (!string.IsNullOrEmpty(prefix))
				val = val.Replace(prefix, "");
			if (!string.IsNullOrEmpty(postfix))
				val = val.Replace(postfix, "");

			if (string.IsNullOrEmpty(val) || textBox.Text == ".")
			{
				value = previousValue;
			}
			else if (hexadecimal && int.TryParse(val, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result))
			{
				value = result;
			}
			else if (decimal.TryParse(val, out decimal decResult))
			{
				value = decResult;
			}
			else
			{
				Debug.WriteLine($"Can't parse {val}");
				return;
			}

			RefreshTextBox();
			Defocus_Click(null, null);
		}

		public void Defocus_Click(object sender, EventArgs e)
		{
			this.ActiveControl = null;
			if (((UserControlParent)ParentForm) == null)
				Debug.WriteLine("Implement UserControlParent on ParentForm to allow for auto-defocus.");
			else
				((UserControlParent)ParentForm).Defocus(sender, e);
		}
	}
}
