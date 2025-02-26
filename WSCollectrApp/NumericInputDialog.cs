using System;
using System.Windows.Forms;

namespace TradingCardManager
{
    public class NumericInputDialog : Form
    {
        private Label lblPrompt;
        private NumericUpDown nudValue;
        private Button btnOK;
        private Button btnCancel;

        public int Value => (int)nudValue.Value;

        public NumericInputDialog(string title, string prompt, int minimum = 1, int maximum = 100)
        {
            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Width = 300;
            Height = 150;

            lblPrompt = new Label
            {
                Text = prompt,
                Left = 20,
                Top = 20,
                Width = 260
            };

            nudValue = new NumericUpDown
            {
                Left = 20,
                Top = 40,
                Width = 260,
                Minimum = minimum,
                Maximum = maximum,
                Value = minimum
            };

            btnOK = new Button
            {
                Text = "OK",
                Left = 110,
                Top = 80,
                Width = 75,
                DialogResult = DialogResult.OK
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                Left = 205,
                Top = 80,
                Width = 75,
                DialogResult = DialogResult.Cancel
            };

            AcceptButton = btnOK;
            CancelButton = btnCancel;

            Controls.Add(lblPrompt);
            Controls.Add(nudValue);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
        }
    }
}