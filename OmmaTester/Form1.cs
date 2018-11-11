using OmmaLicenseValidator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmmaTester
{
    public partial class Form1 : Form
    {
        private readonly LicenseValidator _ommaLiceseValidator;
        public Form1()
        {
            InitializeComponent();
            _ommaLiceseValidator = new LicenseValidator();
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            var result = await _ommaLiceseValidator.Valid(txtLicenseNumber.Text);
            MessageBox.Show(result.ToString());
        }
    }
}
