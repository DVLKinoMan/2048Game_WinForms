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

namespace _2048Game
{
    public partial class Form1 : Form
    {
        public int[,] matrixPosition = new int[4, 4];
        public int[,] lastmatrixPosition = new int[4, 4];
        public int[] randomNumbers = new int[2] { 2, 4 };
        public bool gameWon=false;
        public int score=0;

        public Form1()
        {
            InitializeComponent();
            if (System.IO.File.Exists("savedGame.txt"))
            {
                var result = MessageBox.Show("Do You Want To Resume Last Played Game?", "Saved Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == System.Windows.Forms.DialogResult.Yes)
                    resumeGame();
                else
                {
                    firstRandom();
                    printMatrix();
                    copy(lastmatrixPosition, matrixPosition);
                }
            }
            else
            {
                firstRandom();
                printMatrix();
                copy(lastmatrixPosition, matrixPosition);
            }
            label1.Text = "SCORE " + score.ToString();
            bestScore();
            toolTip1.SetToolTip(this.label1, "Current Score");
            toolTip1.SetToolTip(this.label18, "Your Best Score");
        }

        #region properties
        public void resumeGame()
        {
            if (!System.IO.File.Exists("savedGame.txt"))
            {
                firstRandom();
                printMatrix();
                copy(lastmatrixPosition, matrixPosition);
            }
            else
            {
                using (StreamReader sw = new StreamReader("savedGame.txt"))
                {
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            matrixPosition[i,j]=int.Parse( sw.ReadLine());
                    score = int.Parse(sw.ReadLine());
                    printMatrix();
                    copy(lastmatrixPosition, matrixPosition);
                }
            }
        }
        public void copy(int[,] m, int[,] l)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    m[i, j]=l[i, j] ;
        }
        public void copyTo(int[,] last)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                     last[i, j]=matrixPosition[i, j];
        }

        public bool equal(int[,] last)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (last[i, j] != matrixPosition[i, j])
                    {
                        return false;
                    }
            return true;
        }

        public bool availableMove()
        {
            for (int q = 0; q < 4; q++)
                for (int w = 0; w < 4; w++)
                    if (matrixPosition[q, w] == 0)
                        return true;
            int i = 0, j = 0;
            while (true)
            {
                if (i - 1 >= 0)
                    if (matrixPosition[i - 1, j] == matrixPosition[i, j]) return true;
                if (i + 1 <= 3)
                    if (matrixPosition[i, j] == matrixPosition[i + 1, j]) return true;
                if (j + 1 <= 3)
                    if (matrixPosition[i, j] == matrixPosition[i, j + 1]) return true;
                if (j - 1 >= 0)
                    if (matrixPosition[i, j] == matrixPosition[i, j - 1]) return true;
                j++;
                if (j > 3)
                {
                    j = 0;
                    if (i != 3)
                        i++;
                    else return false;
                }
            }
        }
        #endregion

        #region matrix moves
        public void leftMove()
        {
            copyTo(lastmatrixPosition);
            for (int i = 0; i < 4; i++)
            {
                int k = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (matrixPosition[i, j] != 0)
                    {
                        if (j!=k)
                        {
                            matrixPosition[i, k] = matrixPosition[i, j];
                            matrixPosition[i, j] = 0;
                            k++;
                        }
                        else k++;
                    }
                }
                for (int j = 1; j < 4; j++)
                {
                    if (matrixPosition[i, j] == matrixPosition[i, j - 1])
                    {
                        matrixPosition[i, j - 1] += matrixPosition[i, j];
                        score += matrixPosition[i, j];
                        while (j <= 3)
                        {
                            if (j != 3)
                                matrixPosition[i, j] = matrixPosition[i, j + 1];
                            else matrixPosition[i, j] = 0;
                            j++;
                        }
                        break;
                    }
                }
            }
        }

        public void rightMove()
        {
            copyTo(lastmatrixPosition);
            for (int i = 0; i < 4; i++)
            {
                int k=3;
                for (int j = 3; j >= 0; j--)
                {
                    if (matrixPosition[i, j] != 0)
                    {
                        if (j != k)
                        {
                            matrixPosition[i, k] = matrixPosition[i, j];
                            matrixPosition[i, j] = 0;
                            k--;
                        }
                        else k--;
                    }
                }
                for (int j = 2; j >= 0; j--)
                {
                    if (matrixPosition[i, j] == matrixPosition[i, j + 1])
                    {
                        matrixPosition[i, j + 1] += matrixPosition[i, j];
                        score += matrixPosition[i, j];
                        matrixPosition[i, j] = 0;
                        while (j >= 0)
                        {
                            if (j > 0)
                                matrixPosition[i, j] = matrixPosition[i, j - 1];
                            else matrixPosition[i, j] = 0;
                            j--;
                        }
                        break;
                    }
                }
            }
        }

        public void upMove()
        {
            copyTo(lastmatrixPosition);
            for (int j = 0; j < 4; j++)
            {
                int k = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (matrixPosition[i, j] != 0)
                    {
                        if (i != k)
                        {
                            matrixPosition[k,j] = matrixPosition[i, j];
                            matrixPosition[i, j] = 0;
                            k++;
                        }
                        else k++;
                    }
                }
                for (int i = 1; i < 4; i++)
                {
                    if (matrixPosition[i, j] == matrixPosition[i - 1, j])
                    {
                        matrixPosition[i - 1, j] += matrixPosition[i, j];
                        score += matrixPosition[i, j];
                        while (i <= 3)
                        {
                            if (i != 3)
                                matrixPosition[i, j] = matrixPosition[i + 1, j];
                            else matrixPosition[i, j] = 0;
                            i++;
                        }
                        break;
                    }
                }
            }
        }

        public void downMove()
        {
            copyTo(lastmatrixPosition);
          for (int j = 0; j < 4; j++)
            {
                int k = 3;
                for (int i = 3; i >= 0; i--)
                {
                    if (matrixPosition[i, j] != 0)
                    {
                        if (i != k)
                        {
                            matrixPosition[k, j] = matrixPosition[i, j];
                            matrixPosition[i, j] = 0;
                            k--;
                        }
                        else k--;
                    }
                }
                for (int i = 2; i >= 0; i--)
                {
                    if (matrixPosition[i, j] == matrixPosition[i + 1, j])
                    {
                        matrixPosition[i + 1, j] += matrixPosition[i, j];
                        score += matrixPosition[i , j];
                        while (i >= 0)
                        {
                            if (i != 0)
                                matrixPosition[i, j] = matrixPosition[i - 1, j];
                            else matrixPosition[i, j] = 0;
                            i--;
                        }
                        break;
                    }
                }
            }
        }
        #endregion

        #region colors
        public void printMatrix()
        { 
            List<Label> lb = this.Controls.OfType<Label>().ToList();
            foreach (var lbl in lb)
            {
                for (int k = 2; k <= 17; k++)
                {
                    int i = (k-2) / 4; int j = (k-2) % 4;
                    if (lbl.Name.StartsWith("label" + k.ToString()))
                    {
                        if (matrixPosition[i, j] != 0)
                        {
                            lbl.Text = matrixPosition[i, j].ToString();
                            if (matrixPosition[i, j] == 2 )
                            {
                                lbl.BackColor = Color.Gainsboro;
                                lbl.ForeColor = Color.Black;
                            }
                            if (matrixPosition[i, j] == 4)
                            {
                                lbl.BackColor = Color.LightGray;
                                lbl.ForeColor = Color.Black;
                            }
                            if (matrixPosition[i, j] == 8)
                            {
                                lbl.BackColor = Color.Peru;
                                lbl.ForeColor = Color.White;
                            }
                            if (matrixPosition[i, j] == 16)
                            {
                                lbl.BackColor = Color.Chocolate;
                                lbl.ForeColor = Color.White;
                            }
                            if (matrixPosition[i, j] == 32)
                            {
                                lbl.BackColor = Color.OrangeRed;
                                lbl.ForeColor = Color.White;
                            }
                            if (matrixPosition[i, j] == 64)
                            {
                                lbl.BackColor = Color.Red;
                                lbl.ForeColor = Color.White;
                            }
                            if (matrixPosition[i, j] >= 128 && matrixPosition[i, j] <= 1024)
                            {
                                lbl.BackColor = Color.Goldenrod;
                                lbl.ForeColor = Color.White;
                            }
                            if (matrixPosition[i, j] == 2048)
                            {
                                lbl.BackColor = Color.Gold;
                                lbl.ForeColor = Color.White;
                                if (gameWon==false)
                                {
                                    gameWon = true;
                                    var result = MessageBox.Show("You Won!!! Do You Want Continue?", "Won", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (result == System.Windows.Forms.DialogResult.Yes)
                                        return;
                                    if (result == System.Windows.Forms.DialogResult.No)
                                        Application.Exit();
                                }
 
                            }
                            if (matrixPosition[i, j] > 2048)
                            {
                                lbl.BackColor = Color.Black;
                                lbl.ForeColor = Color.White;
                            }
                        }
                        else
                        {
                            lbl.Text = "";
                            lbl.BackColor = Color.Silver;
                        }
                    }
                }
            }
        }
        #endregion

        #region randoms
        public int randomNumber()
        {
            Random rnd = new Random();
            int first = rnd.Next(2);
            int second = rnd.Next(2);
            int third = rnd.Next(2);
            if (first == second && second == third)
                return randomNumbers[first];
            else return randomNumbers[0];
        }

        public void randomNumberInAction(int k)
        {
            Random rnd = new Random();
            int number = rnd.Next(16);
            int i = number / 4;
            int j = number % 4;
            if (matrixPosition[i, j] == 0)
            {
                matrixPosition[i, j] = k;
                return;
            }
            else
            {
                while (matrixPosition[i, j] != 0)
                {
                    number = rnd.Next(16);
                    i = number / 4;
                    j = number % 4;
                }
            }
            matrixPosition[i, j] = k;
        }

        public void firstRandom()
        {
            Random rnd = new Random();
            int first = rnd.Next(16);
            int last = rnd.Next(16);
            while (last == first)
                last = rnd.Next(16);
            int k = 0;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (k == first || k == last)
                        matrixPosition[i, j] = randomNumber();
                    else matrixPosition[i, j] = 0;
                    k++;
                }
        }
        #endregion


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (availableMove())
            {
                int k;
                if (e.KeyValue == (char)Keys.Left)
                {
                    leftMove();
                    label1.Text = "SCORE " + score.ToString();
                    if(!equal(lastmatrixPosition)){
                    k = randomNumber();
                    randomNumberInAction(k);
                    printMatrix();
                    }
                }
                if (e.KeyValue == (char)Keys.Right)
                {
                    rightMove();
                    label1.Text = "SCORE " + score.ToString();
                    if (!equal(lastmatrixPosition))
                    {
                        k = randomNumber();
                        randomNumberInAction(k);
                        printMatrix();
                    }
                }
                if (e.KeyValue == (char)Keys.Up)
                {
                    upMove();
                    label1.Text = "SCORE " + score.ToString();
                    if (!equal(lastmatrixPosition))
                    {
                        k = randomNumber();
                        randomNumberInAction(k);
                        printMatrix();
                    }
                }
                if (e.KeyValue == (char)Keys.Down)
                {
                    downMove();
                    label1.Text = "SCORE "+score.ToString();
                    if (!equal(lastmatrixPosition))
                    {
                        k = randomNumber();
                        randomNumberInAction(k);
                        printMatrix();
                    }
                }
                
                if (e.KeyValue == (char)Keys.N)
                {
                    var result = MessageBox.Show("Are You Sure You Want To Start New Game?", "New Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                        Application.Restart();
                    if (result == System.Windows.Forms.DialogResult.No)
                        return;
                    GameProfile.deleteSaveGame();
                }
                if (e.KeyValue == (char)Keys.R)
                {
                    var result = MessageBox.Show("Are You Sure You Want To Reset Your Best Score?", "Reset Score", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        GameProfile.Reset();
                        Application.Restart();
                    }
                    if (result == System.Windows.Forms.DialogResult.No)
                        return;
                }
                if (e.KeyValue == (char)Keys.E)
                {
                    var result = MessageBox.Show("Are You Sure You Want To Exit", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                        Application.Exit();
                    if (result == System.Windows.Forms.DialogResult.No)
                        return;
                    GameProfile.saveGame(matrixPosition,score);
                }
                if (e.KeyValue == (char)Keys.G)
                {
                    var result = MessageBox.Show("Try to win the game to score 2048 point", "About Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (result == System.Windows.Forms.DialogResult.OK)
                        return;
                }
                if (e.KeyValue == (char)Keys.A)
                {
                    var result = MessageBox.Show("Author: DaViD :))", "Author", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (result == System.Windows.Forms.DialogResult.OK)
                        return;
                }
                bestScore();
            }
            else
            {
                var result=MessageBox.Show("You Lose!!! Do You Want Play Again?", "Game Over", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question);
                if (result == System.Windows.Forms.DialogResult.Retry)
                    Application.Restart();
                if (result == System.Windows.Forms.DialogResult.Abort)
                    Application.Exit();
                if (result == System.Windows.Forms.DialogResult.Ignore)
                    return;
            }
            if (e.KeyValue == (char)Keys.U)
                {
                    if (!equal(lastmatrixPosition))
                    {
                        copy(matrixPosition, lastmatrixPosition);
                        printMatrix();
                    }
                    else
                    {
                        var result = MessageBox.Show("You Cann't Do Undo Move", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (result == System.Windows.Forms.DialogResult.OK)
                            return;
                    }
                }
        }

        public void bestScore()
        {
            GameProfile.Best(score);
            label18.Text = "BEST   " + GameProfile.bestScore.ToString();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are You Sure You Want To Start New Game?", "New Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
                Application.Restart();
            if (result == System.Windows.Forms.DialogResult.No)
                return;
            GameProfile.deleteSaveGame();
        }

        private void undoMoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!equal(lastmatrixPosition))
            {
                copy(matrixPosition, lastmatrixPosition);
                printMatrix();
            }
            else
            {
                var result = MessageBox.Show("You Cann't Do Undo Move", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.OK)
                    return;
            }
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void authorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Author: DaViD :))", "Author", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (result == System.Windows.Forms.DialogResult.OK)
                return;
        }

        private void gameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Try to win the game to score 2048 point", "About Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (result == System.Windows.Forms.DialogResult.OK)
                return;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are You Sure You Want To Exit", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            Application.Exit();
            if (result == System.Windows.Forms.DialogResult.No)
                return;
        }

        private void resetBestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are You Sure You Want To Reset Your Best Score?", "Reset Score", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                GameProfile.Reset();
                Application.Restart();
            }
            if (result == System.Windows.Forms.DialogResult.No)
                return;

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            GameProfile.saveGame(matrixPosition,score);
        }
    }
}
