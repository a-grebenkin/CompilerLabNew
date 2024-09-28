using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CompilerLab
{
    public partial class Compiler : Form
    {
        public Compiler(OpenFileDialog openFileDialog = null)
        {
            InitializeComponent();
            clearColor = false;
            StringPath = openFileDialog?.FileName??"";
            
            if (openFileDialog == null)
            {
                labelDebug.Text = "Файл создан";
            }
            else
            {

                var fileStream = openFileDialog.OpenFile();

                using (var writer = new StreamReader(fileStream))
                {
                    inputTextBox.Text = writer.ReadToEnd();
                    inputTextBox.SelectionStart = inputTextBox.Text.Length;
                }
                labelDebug.Text = "Файл "+ Path.GetFileName(StringPath) + " открыт";
            }
            initialText = inputTextBox.Text;
        }

        public string StringPath
        {
            get
            {
                return stringPath;
            }
            set
            {
                stringPath = value;
                IsSaved = true;
                SetFormName();
            }
        }

        public bool IsSaved
        { 
            get
            {
                return isSaved;
            }
            set 
            { 
                isSaved = value;
                if (isSaved)
                {
                    initialText = inputTextBox.Text;
                    undoIconToolStripMenuItem.Enabled = false;
                    redoIconToolStripMenuItem.Enabled = false;
                    countUndo = 0;
                }
                SetFormName();
            }
        }

        private bool isSaved;
        private int countUndo;
        private string initialText;
        private string stringPath;
        private bool clearColor;
        private void SetFormName()
        {
            if (IsSaved)
            {
                this.Text = (stringPath == "" ? "Безымянный - Compiler" : Path.GetFileName(StringPath) + " - Compiler");
            }
            else
            {
                this.Text = "*" + (stringPath == "" ? "Безымянный - Compiler" : Path.GetFileName(StringPath) + " - Compiler");
            }
        }

        private void CreateNewWindow(OpenFileDialog openFileDialog = null)
        {
            Compiler compiler = new Compiler(openFileDialog);
            Application.Run(compiler);
        }

        private void Create()
        {
            Thread thread = new Thread(()=>CreateNewWindow());
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            labelDebug.Text = "Файл создан в новом окне";
        }

        private void Open()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Текстовые документы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileStream = openFileDialog.OpenFile();

                    using (var writer = new StreamReader(fileStream))
                    {
                        inputTextBox.Text = writer.ReadToEnd();
                    }
                    StringPath = openFileDialog.FileName;
                    inputTextBox.SelectionStart = inputTextBox.Text.Length;
                    labelDebug.Text = "Файл "+ Path.GetFileName(StringPath) +" открыт в текущем окне";
                }
                else
                {
                    labelDebug.Text = "Открытие файла отменено";
                }
            }
        }

        private void OpenNewTab()
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Текстовые документы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Thread thread = new Thread(() => CreateNewWindow(openFileDialog));
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    labelDebug.Text = "Файл " +Path.GetFileName(openFileDialog.FileName) + " открыт в новом окне";
                }
                else
                {
                    labelDebug.Text = "Открытие файла отменено";
                }
            }
        }

        private byte WouldSaveFile()
        {
            var result = MessageBox.Show("Сохранить изменения?", "Compiler", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
                return 2;
            if (result == DialogResult.No)
                return 1;
            return 0;
        }


        private void Save()
        {
            if (StringPath == "")
            {
                SaveAs();
            }
            else
            {
                using (FileStream fs = File.Create(StringPath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(inputTextBox.Text);
                    fs.Write(info, 0, info.Length);
                }
            }
            IsSaved = true;
            labelDebug.Text = "Файл сохранен";
        }

        private void SaveAs()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "Текстовые документы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                saveFileDialog.FileName = Path.GetFileNameWithoutExtension(stringPath);

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {

                    var fileStream = saveFileDialog.OpenFile();

                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.Write(inputTextBox.Text);
                    }

                    StringPath = saveFileDialog.FileName;
                    labelDebug.Text = "Файл сохранен как " + Path.GetFileName(StringPath);
                }
                else
                {
                    labelDebug.Text = "Сохранение файла отменено";
                }
            }
        }

        private void Undo()
        {
            countUndo++;
            inputTextBox.Undo();
            labelDebug.Text = "Операция отменена";
        }

        private void Redo()
        {
            countUndo = countUndo > 0 ? countUndo - 1:0;
            inputTextBox.Redo();
            labelDebug.Text = "Операция возвращена";
        }

        private void Reference()
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FileName = string.Format("{0}Resources\\menu.html", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            Help.ShowHelp(this, FileName);
        }

        private void AboutTool()
        {
            string text = "Авторы приложения:\nСтуденты группы АВТ-913:\nАнуфриев И.В., Гребенкин А.C., Малахов Д.Е..";
            MessageBox.Show(text, "О программе");
            labelDebug.Text = "Открыта информация о прогамме";
        }

       

        private void СompilationNEW()
        {
            outputTextBox.Text = AntlerHelper.GetResult(inputTextBox.Text);
           
            labelDebug.Text = "Выполнен разбор с помощью Antler";
        }

        private void Сompilation()
        {
            outputTextBox.Text = RecursiveDescent.GetResult(inputTextBox.Text);

            labelDebug.Text = "Выполнен разбор с помощью рекурсивного спуска";
        }

        private void CreateIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Create();
        }

        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Create();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputTextBox.Text.Length == 0)
            {
                Open();
            }
            else
            {
                OpenNewTab();
            }
        }

        private void OpenIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputTextBox.Text.Length == 0)
            {
                Open();
            }
            else
            {
                OpenNewTab();
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void SaveIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }
        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void UndoIconToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void RedoIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }
        
        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputTextBox.Cut();
            labelDebug.Text = "Текст вырезан";
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputTextBox.Copy();
            labelDebug.Text = "Текст скопирован";
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputTextBox.Paste();
            labelDebug.Text = "Текст вставлен";
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputTextBox.Clear();
            labelDebug.Text = "Поле ввода очищено";
        }

        private void SelectAllВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputTextBox.SelectAll();
            labelDebug.Text = "Весь текст выделен";
        }

        private void CopyIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputTextBox.Copy();
            labelDebug.Text = "Текст скопирован";
        }

        private void CutIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputTextBox.Cut();
            labelDebug.Text = "Текст вырезан";
        }

        private void PasteIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputTextBox.Paste();
            labelDebug.Text = "Текст вставлен";
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (clearColor)
            {
                SuspendLayout();
                int startSelection = inputTextBox.SelectionStart;
                int startLength = inputTextBox.SelectionLength;
                inputTextBox.SelectionStart = 0;
                inputTextBox.SelectionLength = inputTextBox.Text.Length;
                inputTextBox.SelectionColor = Color.Black;
                inputTextBox.SelectionStart = startSelection;
                inputTextBox.SelectionLength = startLength;
                ResumeLayout();
                clearColor = false;
            }

        }

        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            if (inputTextBox.Text != initialText)
                IsSaved = false;
            undoIconToolStripMenuItem.Enabled = inputTextBox.Text != initialText;
            redoIconToolStripMenuItem.Enabled = countUndo > 0;
        }


        private void PlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }


        private void ReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reference();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutTool();
        }


        private void PlayIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            СompilationNEW();
        }

        private void DeleteIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputTextBox.Clear();
            labelDebug.Text = "Поле ввода очищено";
        }

        private void Compiler_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                byte result = WouldSaveFile();
                if (result == 2)
                {
                    Save();
                }
                if (result == 0)
                {
                    e.Cancel = true;
                    labelDebug.Text = "Закрытие файла отменено";
                }
            }
        }

        private void Text1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo("kr.pdf");
            Process.Start(new ProcessStartInfo("cmd", $"/c start microsoftedge file://" + fileInfo.FullName + "#page=3"));
        }

        private void Text2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo("kr.pdf");
            Process.Start(new ProcessStartInfo("cmd", $"/c start microsoftedge file://" + fileInfo.FullName + "#page=4"));
        }

        private void Text3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo("kr.pdf");
            Process.Start(new ProcessStartInfo("cmd", $"/c start microsoftedge file://" + fileInfo.FullName + "#page=5"));
        }

        private void Text4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo("kr.pdf");
            Process.Start(new ProcessStartInfo("cmd", $"/c start microsoftedge file://" + fileInfo.FullName + "#page=6"));
        }

        private void Text5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo("kr.pdf");
            Process.Start(new ProcessStartInfo("cmd", $"/c start microsoftedge file://" + fileInfo.FullName + "#page=7"));
        }

        private void Text6ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo("kr.pdf");
            Process.Start(new ProcessStartInfo("cmd", $"/c start microsoftedge file://" + fileInfo.FullName + "#page=8"));
        }

        private void Text7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo("kr.pdf");
            Process.Start(new ProcessStartInfo("cmd", $"/c start microsoftedge file://" + fileInfo.FullName + "#page=10"));
        }

        private void Text8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo("kr.pdf");
            Process.Start(new ProcessStartInfo("cmd", $"/c start microsoftedge file://" + fileInfo.FullName + "#page=15"));
        }

        private void aNTLERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            СompilationNEW();
        }

        private void рекурсивныйСпускToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Сompilation();
        }
    }
}
