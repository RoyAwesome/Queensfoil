using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queensfoil.Pkg
{
    public static class Oodle
    {
        const string BinPath = "bin\\x64\\";
        const string OodleDllName = "oo2core_3_win64.dll";
        private static string OodleDll;

        static Oodle()
        {
            Util.OnDestiny2FolderSet += InitOodle;
            InitOodle();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SOodleDecodeInfo
        {
            UInt32 OutputGenerated;
            UInt32 InputConsumed;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            UInt32[] B;
        }

        public delegate Int32 OodleLZDecoder_MemorySizeNeededDel(UInt32 A, Int64 B);
        public delegate IntPtr OodleLZDecoder_CreateDel(Int32 A, Int64 B, IntPtr Buffer, UInt64 /*size_t*/ Size);
        public delegate UInt32 OodleLZDecoder_DecodeSomeDel(IntPtr Decoder, SOodleDecodeInfo info, IntPtr a, UInt32 b, UInt64 /*size_t*/ c,
            UInt64 /*size_t*/ d, IntPtr e, UInt64 /*size_t*/ f, UInt32 g, UInt32 h, UInt32 i, UInt32 j);
        public delegate void OodleLZDecoder_DestroyDel(IntPtr Decoder);

        private static IntPtr OodleDllPtr = IntPtr.Zero;

        public static OodleLZDecoder_MemorySizeNeededDel MemorySizeNeeded;
        public static OodleLZDecoder_CreateDel Create;
        public static OodleLZDecoder_DecodeSomeDel DecodeSome;
        public static OodleLZDecoder_DestroyDel Destroy;


        public static void InitOodle()
        {
            if(!Util.HasValidD2Directory)
            {
                return;
            }

            OodleDll = Path.Combine(Util.D2Path, BinPath, OodleDllName);
            if(!File.Exists(OodleDll))
            {
                MessageBox.Show("Cannot find Oodle dll.  This means we cannot load pkg files. \n\n Make sure oo2core_3_win64.dll exists in Destiny 2/bin/x64", "Error", MessageBoxButtons.OK);
                return;
            }

            if(OodleDllPtr != IntPtr.Zero)
            {
                DllUtil.FreeLibrary(OodleDllPtr);
            }

            OodleDllPtr = DllUtil.LoadLibrary(OodleDll);

            if(OodleDllPtr == IntPtr.Zero)
            {
                MessageBox.Show($"Cannot Load the Oodle Library! {Marshal.GetLastWin32Error()}", "Error", MessageBoxButtons.OK);
                return;
            }

            {
                IntPtr FuncPtr = DllUtil.GetProcAddress(OodleDllPtr, "OodleLZDecoder_MemorySizeNeeded");
                if (FuncPtr == IntPtr.Zero)
                {
                    MessageBox.Show("Cannot Load OodleLZDecoder_MemorySizeNeeded.", "Error", MessageBoxButtons.OK);
                    return;

                }
                MemorySizeNeeded = (OodleLZDecoder_MemorySizeNeededDel)Marshal.GetDelegateForFunctionPointer(FuncPtr,
                    typeof(OodleLZDecoder_MemorySizeNeededDel));
            }

            {
                IntPtr FuncPtr = DllUtil.GetProcAddress(OodleDllPtr, "OodleLZDecoder_Create");
                if (FuncPtr == IntPtr.Zero)
                {
                    MessageBox.Show("Cannot Load OodleLZDecoder_Create.", "Error", MessageBoxButtons.OK);
                    return;

                }
                Create = (OodleLZDecoder_CreateDel)Marshal.GetDelegateForFunctionPointer(FuncPtr,
                    typeof(OodleLZDecoder_CreateDel));
            }

            {
                IntPtr FuncPtr = DllUtil.GetProcAddress(OodleDllPtr, "OodleLZDecoder_Destroy");
                if (FuncPtr == IntPtr.Zero)
                {
                    MessageBox.Show("Cannot Load OodleLZDecoder_Destroy.", "Error", MessageBoxButtons.OK);
                    return;

                }
                Destroy = (OodleLZDecoder_DestroyDel)Marshal.GetDelegateForFunctionPointer(FuncPtr,
                    typeof(OodleLZDecoder_DestroyDel));
            }

            {
                IntPtr FuncPtr = DllUtil.GetProcAddress(OodleDllPtr, "OodleLZDecoder_DecodeSome");
                if (FuncPtr == IntPtr.Zero)
                {
                    MessageBox.Show("Cannot Load OodleLZDecoder_DecodeSome.", "Error", MessageBoxButtons.OK);
                    return;

                }
                DecodeSome = (OodleLZDecoder_DecodeSomeDel)Marshal.GetDelegateForFunctionPointer(FuncPtr,
                    typeof(OodleLZDecoder_DecodeSomeDel));
            }


        }
    }
}
