using System;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace AWS.SMTP.CredentialGenerator
{
    public partial class keygen : Form
    {
        //http://docs.aws.amazon.com/ses/latest/DeveloperGuide/smtp-credentials.html#smtp-credentials-convert

        private static byte VERSION = 0x02; // Version number. Do not modify.
        private static byte[] MESSAGE; // Used to generate the HMAC signature. Do not modify.

        private Timer m_ClipboardTimer;

        public keygen()
        {
            InitializeComponent();
            this.m_ClipboardTimer = new Timer()
            {
                Interval = 1000
            };
            this.m_ClipboardTimer.Tick += ClipboardTimer_Tick;
            this.linkLabel1.Links.Add(0, 32, "https://awscredgen.codeplex.com");
        }

        void ClipboardTimer_Tick(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(this.lblClipboardCountdown.Text);

            if (count == 1)
            {
                this.lblClipboardCountdown.Text = string.Empty;
                this.m_ClipboardTimer.Stop();
                Clipboard.Clear();
            }
            else
            {
                this.lblClipboardCountdown.Text = (count - 1).ToString();
            }
        }

        static keygen()
        {
            keygen.MESSAGE = System.Text.Encoding.ASCII.GetBytes("SendRawEmail");
        }

        public static string GenerateCredentials(string awsSecretKey)
        {
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(awsSecretKey);

            // Create an HMAC-SHA256 key from the raw bytes of the AWS secret access key.
            HMACSHA256 secretKey = new HMACSHA256(bytes);

            byte[] rawSignature = secretKey.ComputeHash(keygen.MESSAGE);

            // Prepend the version number to the signature.
            byte[] rawSignatureWithVersion = new byte[rawSignature.Length + 1];

            byte[] versionArray = { VERSION };
            Array.Copy(versionArray, 0, rawSignatureWithVersion, 0, 1);
            Array.Copy(rawSignature, 0, rawSignatureWithVersion, 1, rawSignature.Length);
            return Convert.ToBase64String(rawSignatureWithVersion);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.txtPassword.Text = keygen.GenerateCredentials(this.txtSecretKey.Text);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.txtPassword.Text);
            this.m_ClipboardTimer.Start();
            this.lblClipboardCountdown.Text = "30";
        }

        private void keygen_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clipboard.Clear();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
    }
}
