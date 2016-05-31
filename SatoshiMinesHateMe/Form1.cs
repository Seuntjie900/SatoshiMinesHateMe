using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatoshiMinesHateMe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("dis awesome tool was made by hesapadim\ndonate my address on bitcointalk if you want\nplease do it lol\n\nhow does dis awesome thing work\nbet previous mine locations\nif lose 3x bet and do it again\n\nnote:you should have at least base x 10 balance\n\n\nnothing is guaranteed but its winning\n\nwarnings\nthis is gambling. gambling = lose\ni made this tool for myself, you can reverse source easily and modify it\nif you dont donate me satoshimines wont like you\n\n\nnote: satoshimines url will be written in bottom dont lose it or everything will gone\nnote 2: if you need force stop just exit app\nnote 3: this tool is not guaranteed");
            
        }
        async void creategame(int amount)
        {
            webBrowser1.Document.GetElementById("bet").SetAttribute("value", amount.ToString());
            webBrowser1.Document.GetElementById("start_game").InvokeMember("click");
        waiting:
            await Task.Delay(1000);
           if(webBrowser1.Document.GetElementById("start_game").GetAttribute("disabled") == "disabled")
           {
               goto waiting;
           }
           await Task.Delay(200);
        }
        async void cashout()
        {
            string currgame = "";
            foreach (HtmlElement game in webBrowser1.Document.GetElementsByTagName("div"))
            {
                if (game.GetAttribute("className").Contains("game ") && game.Id.StartsWith("game_") && !game.InnerText.Contains("Game over!") && !game.InnerText.Contains("Cashed out "))
                {
                    currgame = game.Id;
                    foreach (HtmlElement elm in game.Document.GetElementsByTagName("button"))
                    {
                        if (elm.GetAttribute("className") == "cashout")
                        {
                            elm.InvokeMember("click");
                            break;
                        }
                    }
                }
            }
        waiting:
            await Task.Delay(1000);

        try
        {
            foreach (HtmlElement elm in webBrowser1.Document.GetElementById(currgame).Document.GetElementsByTagName("button"))
            {
                if (elm.GetAttribute("className") == "cashout")
                {
                    if (elm.GetAttribute("disabled") == "disabled")
                    {
                        goto waiting;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        catch
        {
            goto waiting;
        }
        }
        async void press(int id)
        {
            string currentgame = "";
            bool brk = false;
            foreach (HtmlElement game in webBrowser1.Document.GetElementsByTagName("div"))
            {
                if (game.GetAttribute("className").Contains("game ") && game.Id.StartsWith("game_") && !game.InnerText.Contains("Game over!"))
                {
                    currentgame = game.Id;
                    foreach (HtmlElement elm in game.Document.GetElementsByTagName("li"))
                    {
                        if (elm.GetAttribute("className") == "tile" && elm.GetAttribute("data-tile") == id.ToString())
                        {
                            elm.InvokeMember("click");
                            brk = true;
                            break;
                        }
                    }
                }
                if (brk) { break; }
            }
        waiting:
            await Task.Delay(1000);
        try {
            foreach (HtmlElement elm in webBrowser1.Document.GetElementById(currentgame).Document.GetElementsByTagName("li"))
            {
                if (elm.GetAttribute("className") == "tile" && elm.GetAttribute("data-tile") == id.ToString())
                {
                    if (elm.GetAttribute("className").Contains("active_tile"))
                    {
                        goto waiting;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        catch { goto waiting;  }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            textBox3.Text = webBrowser1.Url.ToString();
        }










        private int[] lastMines()
        {
            int[] mines = new int[25];
            var lpc = 0;
            foreach (HtmlElement game in webBrowser1.Document.GetElementsByTagName("div"))
            {
                if (game.GetAttribute("className").Contains("game ") && game.Id.StartsWith("game_") && (game.InnerText.Contains("Cashed out ") || game.InnerText.Contains("Game over!")))
                {
                    foreach (HtmlElement elm in game.Document.GetElementsByTagName("li"))
                    {
                        if (elm.GetAttribute("className") == "tile reveal")
                        {
                            if (lpc == 3)
                            {
                                break;
                            }
                            mines[lpc] = Convert.ToInt32(elm.GetAttribute("data-tile"));
                            lpc++;
                        }
                    }
                    break;
                }
            }
            return mines;
        }
        bool stop = false;
        private async void button1_Click(object sender, EventArgs e)
        {
            stop = false;
            creategame(Convert.ToInt32(textBox1.Text));
            await Task.Delay(1500);
            cashout();
            await Task.Delay(1500);
            loop:
            if (stop) { goto finish; }
            await Task.Delay(1500);
            if (stop) { goto finish; }
            foreach(int tile in lastMines())
            {
                if (stop) { goto finish; }
                press(tile);
                if(tile > 0)
                {
                    if (stop) { goto finish; }
                await Task.Delay(1500);

                }
            }
            if (stop) { goto finish; }
            await Task.Delay(1500);
            if (stop) { goto finish; }
            cashout();
            if (stop) { goto finish; }
            await Task.Delay(1500);
            bool lost = false;
            foreach (HtmlElement game in webBrowser1.Document.GetElementsByTagName("div"))
            {
                if (game.GetAttribute("className").Contains("game ") && game.Id.StartsWith("game_") && (game.InnerText.Contains("Cashed out ") || game.InnerText.Contains("Game over!")))
                {
                    if (game.InnerText.Contains("Cashed out "))
                    {
                        lost = false;
                    }
                    else
                    {
                        lost = true;
                    }
                    break;
                }
            }
            if (stop) { goto finish; }
            await Task.Delay(1500);
            if (stop) { goto finish; }
            foreach(HtmlElement he in webBrowser1.Document.GetElementsByTagName("span"))
            {
                if (he.GetAttribute("className") == "num")
                {
                    if (Convert.ToInt32(textBox2.Text) < Convert.ToInt32(he.InnerText.Replace(",", "")))
                    {
                        stop = true;
                    }
                }
            }
            if (lost)
            {
                creategame(Convert.ToInt32(webBrowser1.Document.GetElementById("bet").GetAttribute("value")) * 3);
            }
            else
            {
                creategame(Convert.ToInt32(textBox1.Text));
            }
            await Task.Delay(1500);
            goto loop;
        finish:
            int x = 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            stop = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cashout();
        }
    }
}
