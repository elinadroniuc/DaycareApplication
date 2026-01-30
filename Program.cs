using System;
using System.Windows.Forms;

namespace DaycareApplication
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (AuthorisationForm authForm = new AuthorisationForm())
            {
                if (authForm.ShowDialog() == DialogResult.OK)
                {
                    var mainForm = new MainForm(authForm.LoggedInUser, authForm.LoggedInRole);

                    if (mainForm.AccessGranted)
                    {
                        Application.Run(mainForm);
                    }
                    else
                    {
                        MessageBox.Show("Доступ запрещён. Программа будет закрыта.");
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
        }
        }
    }
