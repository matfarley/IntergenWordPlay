/* Author: Matthew Farley
 * Date: 12 June 2013
 * 
 * This form is the main view for the application and only has minimal control.  
 * The methods for loading the files are coded into it, but otherwise it is only
 * making calls to the Parsing Engine's methods.
 * Exceptions are also handled in this class/form.
 */ 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace WordPlay
{
    public partial class frmMain : Form
    {
        TextReader tr;
        ParsingEngine pE;

        public frmMain()
        {
            InitializeComponent();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //Brings up the load file dialog and loads file into viewing window 
        private void mnuLoad_Click(object sender, EventArgs e)
        {
            opnFile.Filter = "Text Files (*.*)|*.txt";
            opnFile.ShowDialog();
            if (opnFile.FileName != "")
            {
                try
                {
                    tr = File.OpenText(opnFile.FileName);
                    txtDisplayFile.Text = tr.ReadToEnd();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Loading Text File", "File Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        //All of the Parsing Engines methods are called from this event handler
        private void button1_Click(object sender, EventArgs e)
        {
            int noOfWords;
            int noOfSentences;
            pE = new ParsingEngine();

            txtOutput.Clear();
             
            // Returns the Number of Sentences figure and assigns it.
            noOfSentences = pE.CreateSentenceArray(txtDisplayFile.Text).Length;
            txtOutput.Text += "No of sentences: " + noOfSentences.ToString() + "\r\n";
            
            // Returns the Number of words figure and assigns it.
            noOfWords = pE.CreateWordArray(txtDisplayFile.Text).Length;
            txtOutput.Text += "No of words: " + noOfWords.ToString() + "\r\n";

            // Returns the longest sentence.  THROWS EXCEPTIONS IF NO FILE IS LOADED.
            try
            {
                String longestSentence = pE.FindLongestSentence().ToString();
                int longestIndex = pE.getLongestIndex() + 1;
                txtOutput.Text += "Sentence with most words: Sentence no." + longestIndex + ", " + longestSentence + "\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Must Load a File", "No File Loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Returns most frequently used word
            pE.FindMostFrequentWord();
            txtOutput.Text += "Most frequently used word: " + pE.getMostFrequentWord() + "\r\n";
            
            
            //Returns xx longest word
            pE.findXXLongestWord();
            txtOutput.Text += "3rd longest word(s): " + pE.getWordLenghtOutput();   
        }
    }
}
