using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

#if !XBOX
using System.Windows.Forms;
#endif

using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class DialogManager
    {
        public bool InUse = false;
        public bool FileOpenSave = false;
        public bool FileOpenLoad = false;


#if !XBOX
        public void Load()
        {

            FileOpenLoad = true;


            Stream MyStream;
            OpenFileDialog openFileDialog1;

            openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Level files (*.lvl)|*.lvl";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            

            InUse = true;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //try
                {
                    if ((MyStream =File.Open(openFileDialog1.FileName,FileMode.Open)) != null)
                    {
                        using (MyStream)
                        {
                            MasterEditor.LoadNewLevel(ReadFile(new BinaryReader(MyStream)));
                        }
                    }
                }
                //catch (Exception ex)
                {
                //    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            InUse = false;
        }

        public void Save()
        {
            FileOpenSave = true;

            Stream MyStream;
            SaveFileDialog openFileDialog1;

            openFileDialog1 = new SaveFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Level files (*.lvl)|*.lvl";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.OverwritePrompt = true;

            InUse = true;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (File.Exists(openFileDialog1.FileName))
                        File.Delete(openFileDialog1.FileName);
                    if ((MyStream = File.Create(openFileDialog1.FileName)) != null)
                    {
                        using (MyStream)
                        {
                            WriteFile(new BinaryWriter(MyStream), GameManager.MyLevel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            InUse = false;
        }
#endif

        public void OpenDialog()
        {
            

        }


        void WriteFile(BinaryWriter Writer,Level level)
        {
            WriteRectangle(Writer, level.MyRectangle);
            WriteVector4(Writer, level.MyCamera.MyRectangle);
            WriteVector2(Writer, level.MyCamera.EditorOffset);


            Writer.Write((Int32)(level.ObjectList.Count+level.TimeEvents.Count));

            foreach (BasicObject Object in level.ObjectList)
            {
                Object.PreWrite(Writer);
                Object.Write(Writer);
            }

            foreach (BasicObject Object in level.TimeEvents)
            {
                Object.PreWrite(Writer);
                Object.Write(Writer);
            }
            Writer.Write((Int32)level.MyBackground.BackgroundNumber);
        }

        public static Level ReadFile(BinaryReader Reader)
        {
            Level NewLevel = new Level(ReadRectangle(Reader),new Camera(ReadVector4(Reader)));
            NewLevel.MyCamera.EditorOffset = ReadVector2(Reader);

            int ObjectCount=Reader.ReadInt32();
            NewLevel.ObjectList = new List<BasicObject>(ObjectCount);

            for (int i = 0; i < ObjectCount; i++)
            {
                BasicObject NewObject = Instancer.CreateInstanceOf(Reader.ReadString(),NewLevel);
                NewObject.Create(Vector2.Zero,Vector2.Zero);
               // if (NewObject != null)
                {
                    NewObject.PreRead(Reader);
                    NewObject.Read(Reader);
                }
            }
            try
            {
                NewLevel.MyBackground = BackgroundBasic.ReturnBackground(Reader.ReadInt32());
            }
            catch (Exception e)
            {
            }
            return NewLevel;

        }

        public void Update()
        {
            

        }

        public static void WriteRectangle(BinaryWriter Writer, Rectangle Rect)
        {
            Writer.Write((Int32)Rect.X);
            Writer.Write((Int32)Rect.Y);
            Writer.Write((Int32)Rect.Width);
            Writer.Write((Int32)Rect.Height);
        }

        public static Rectangle ReadRectangle(BinaryReader Reader)
        {
            return new Rectangle(Reader.ReadInt32(), Reader.ReadInt32(), Reader.ReadInt32(), Reader.ReadInt32());
        }

        public static void WriteVector2(BinaryWriter Writer, Vector2 Input)
        {
            Writer.Write((Single)Input.X);
            Writer.Write((Single)Input.Y);
        }

        public static Vector2 ReadVector2(BinaryReader Reader)
        {
            return new Vector2(Reader.ReadSingle(), Reader.ReadSingle());
        }

        public static void WriteVector4(BinaryWriter Writer, Vector4 Input)
        {
            Writer.Write((Single)Input.X);
            Writer.Write((Single)Input.Y);
            Writer.Write((Single)Input.Z);
            Writer.Write((Single)Input.W);
        }

        public static Vector4 ReadVector4(BinaryReader Reader)
        {
            return new Vector4(Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle());
        }

        public static void WriteColor(BinaryWriter Writer, Color Input)
        {
            Writer.Write((Single)Input.R);
            Writer.Write((Single)Input.G);
            Writer.Write((Single)Input.B);
            Writer.Write((Single)Input.A);
        }

        public static Color ReadColor(BinaryReader Reader)
        {
            return new Color(Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle());
        }
    }
}
