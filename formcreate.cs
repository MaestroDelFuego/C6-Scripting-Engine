using System;
using System.Windows.Forms;
using System.Drawing;

var helloWorldForm = CreateForm("Hello World");

var label = new Label
{
    Text = "Welcome to the Hello World Form!",
    Dock = DockStyle.Top,
    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
};

var button = new Button
{
    Text = "Close",
    Dock = DockStyle.Bottom
};

// Add a click event to the button to close the form
button.Click += (s, e) => helloWorldForm.Close();

helloWorldForm.Controls.Add(label);
helloWorldForm.Controls.Add(button);

// Show the newly created form
ShowForm(helloWorldForm);