using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Vorbis;



namespace MusicPlay
{
    public partial class Form1 : Form
    {
        string[] files;

        List<string> localmusiclist = new List<string> { };
        public Form1()
        {
            InitializeComponent();
        }

        private void musicplay(string filename)
        {
            string extension = Path.GetExtension(filename);
            if (extension == ".ogg") { Console.WriteLine("this is an ogg file"); }
            else
            {
                Console.WriteLine("this is not an ogg file");
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }

            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "选择音频|*.mp3;*.wav;*.flac";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();//需要把之前的清空
                if (files != null)
                {
                    Array.Clear(files, 0, files.Length);//利用array做范类
                }
                files = openFileDialog1.FileNames;

                string[] array = files;//对于数组信息进行管理

                foreach (string x in array)
                {
                    listBox1.Items.Add(x);
                    localmusiclist.Add(x);//把音乐文件的路径添加到list中
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (localmusiclist.Count > 0)
            {
                axWindowsMediaPlayer1.URL = localmusiclist[listBox1.SelectedIndex];
                musicplay(axWindowsMediaPlayer1.URL);
                音乐播放器.Text = Path.GetFileName(localmusiclist[listBox1.SelectedIndex]);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = trackBar1.Value;
            label2.Text = trackBar1.Value + "%";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int nextIndex = listBox1.SelectedIndex + 1;
            if (nextIndex >= localmusiclist.Count)
            {
                nextIndex = 0;
            }

            axWindowsMediaPlayer1.URL = localmusiclist[nextIndex];
            musicplay(axWindowsMediaPlayer1.URL);
            音乐播放器.Text = Path.GetFileNameWithoutExtension(localmusiclist[nextIndex]);
            listBox1.SelectedIndex = nextIndex;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "打开音频|*.ogg";

            string oggFilePath = "";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                oggFilePath = openFileDialog.FileName;
            }

            using (var vorbisReader = new VorbisWaveReader(oggFilePath))
            {
                // 创建一个WaveOutEvent实例来播放音频  
                using (var waveOut = new WaveOutEvent())
                {
                    // 初始化WaveOutEvent实例，并设置其输入为VorbisFileReader  
                    waveOut.Init(vorbisReader);

                    // 开始播放音频  
                    waveOut.Play();

                    // 等待播放完成，或者可以添加其他逻辑，比如响应播放事件  
                    while (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100); // 等待一段时间，避免死循环  
                    }

                    // 播放完成后，停止并释放WaveOutEvent资源  
                    waveOut.Stop();
                    waveOut.Dispose();
                }

                // 释放VorbisFileReader资源  
                vorbisReader.Dispose();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
