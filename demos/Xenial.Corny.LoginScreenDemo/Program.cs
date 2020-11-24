using System;

using Xenial.Delicious.Corny;

namespace Xenial.Corny.LoginScreenDemo
{
    internal static class Program
    {
        internal static void Main(string[] _)
        {
            var txt1 = new Textbox
            {
                Name = "usernameTextbox",
                Location = new System.Drawing.Point(10, 0),
                Width = 25,
            };

            var txt2 = new Textbox
            {
                Name = "passwordTextbox",
                Location = new System.Drawing.Point(10, 2),
                Width = 25,
                IsPassword = true,
            };

            var form = new Form
            {
                Title = "My Form 123",
                Location = new System.Drawing.Point(4, 2),
                Size = new System.Drawing.Size(40, 20),
                Controls =
                {
                    new Label
                    {
                        Name = "usernameLabel",
                        Text = "Username:",
                        BackColor = ConsoleColor.Black,
                        ForeColor = ConsoleColor.White,
                        Location = new System.Drawing.Point(0, 0),
                    },
                    new Label
                    {
                        Name = "passwordLabel",
                        Text = "Password:",
                        BackColor = ConsoleColor.Black,
                        ForeColor = ConsoleColor.White,
                        Location = new System.Drawing.Point(0, 2),
                    },
                    txt1,
                    txt2,
                    new Label
                    {
                        Name = "rememberLabel",
                        Text = "Remember:",
                        BackColor = ConsoleColor.Black,
                        ForeColor = ConsoleColor.White,
                        Location = new System.Drawing.Point(0, 4),
                    },
                    new Checkbox
                    {
                        Name = "rememberCheckbox",
                        Location = new System.Drawing.Point(10, 4),
                        Text = "Remember Login"
                    },
                    new Label
                    {
                        Name = "optionsLabel",
                        Text = "Options:",
                        BackColor = ConsoleColor.Black,
                        ForeColor = ConsoleColor.White,
                        Location = new System.Drawing.Point(0, 6),
                    },
                    new RadioGroup
                    {
                        Name = "radioGroup",
                        Location = new System.Drawing.Point(10, 6),
                        Options =
                        {
                            "Admin",
                            "Mananger",
                            "User"
                        },
                        Checked = "User"
                    },
                    new Button
                    {
                        Name = "cancelButton",
                        Location = new System.Drawing.Point(10, 10),
                        Text = "Cancel",
                        Width = 12
                    },
                    new Button
                    {
                        Name = "okButton",
                        Location = new System.Drawing.Point(23, 10),
                        Text = "Ok",
                        Width = 12
                    },
                }
            };

            var screen = new Screen
            {
                Renderables =
                {
                    form,
                }
            };

            screen.SetSize(50, 120);

            var left = (screen.Width / 2) - form.Size.Width / 2;

            var top = ((screen.Height / 2) - form.Size.Height / 2) - 1;

            form.Location = new System.Drawing.Point(left, top);

            form.Focus();

            screen.Run();
        }
    }
}
