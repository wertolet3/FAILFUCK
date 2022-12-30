using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Threading;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using System.Media;

namespace Failcodes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form_FormClosing;
            timer1.Tick += timer1_Tick;
            this.Icon = FAILFUCK.Properties.Resources.coupon;
            this.Text = "FAILFUCK v2";
        }

        EdgeDriver driver;
        SoundPlayer player;
        String promo_code = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            var options = new EdgeOptions();

            options.BinaryLocation = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
            new DriverManager().SetUpDriver(new EdgeConfig());

            options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            options.AddUserProfilePreference("profile.default_content_setting_values.stylesheets", 2);
            options.AddArgument("--no-default-browser-check");
            options.AddArgument("start-maximized");
            options.AddArgument("disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-application-cache");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-dev-shm-usage");

            var edgeDriverService = EdgeDriverService.CreateDefaultService();
            edgeDriverService.HideCommandPromptWindow = true;
            driver = new EdgeDriver(edgeDriverService, options);
            driver.Manage().Window.Size = new Size(400, 800);
            driver.Manage().Window.Minimize();
            try
            {
                
                driver.Url = "https://csfail.org/uk/bonuses";
                Thread.Sleep(1000);
                driver.FindElement(By.XPath("/html/body/app-root/shell-wrapper/shell-handheld-toolbar/div/div[1]/button[2]")).Click();
                richTextBox1.Text += DateTime.Now.ToString("HH:mm") + " Завантажено. Очікування промокодів.";
                timer1.Start();
            }
            catch { }

            
        }

        void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                driver.Quit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                var uses = driver.FindElements(By.ClassName("promo__uses"));
                var code = driver.FindElements(By.ClassName("promo__code"));
                var type = driver.FindElements(By.ClassName("promo__type"));
                if (code.Any() && type.Any())
                {
                    if (code.Last().Text.Length > 1 && type.Last().Text.Length > 1)
                    {
                        if (promo_code != code.Last().Text)
                        {
                            promo_code = code.Last().Text;
                            Clipboard.SetText(promo_code);
                            if (type.Last().Text.ToLower().Contains("обичка"))
                                player = new SoundPlayer(FAILFUCK.Properties.Resources.miui_12_notification);
                            if (type.Last().Text.ToLower().Contains("спец"))
                                player = new SoundPlayer(FAILFUCK.Properties.Resources.iphone_notification);

                            player.Play();
                            richTextBox1.Text += "\n" + DateTime.Now.ToString("HH:mm") + " " + promo_code + " (" + type.Last().Text + ") " + uses.Last().Text;
                        }
                    }
                }
                var ping = driver.FindElement(By.ClassName("online__title"));
                var online = driver.FindElement(By.ClassName("online__count"));
                label1.Text = ping.Text + ", ONLINE " + online.Text;

                var message = driver.FindElements(By.ClassName("message__text"));
                if(message.Any())
                {
                    if(pictureBox1.Image != FAILFUCK.Properties.Resources.green)
                    {
                        pictureBox1.Image = FAILFUCK.Properties.Resources.green;
                    }
                }
                else
                {
                    if (pictureBox1.Image != FAILFUCK.Properties.Resources.red)
                    {
                        pictureBox1.Image = FAILFUCK.Properties.Resources.red;
                    }
                }
            }
            catch { 
            }
        }

    }
}
