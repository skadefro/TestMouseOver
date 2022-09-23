using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using static TestMouseOver.NativeMethods;
using System.Windows.Automation;
using FlaUI.Core.Definitions;
using System.Diagnostics;
using System.Xml.Linq;

namespace TestMouseOver
{
    internal class Program
    {

        private static NativeMethods.LLProc mouseProc;
        private static IntPtr mouseHook;
        public static bool doCrash = true;
        private static IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if ((int)wParam == NativeMethods.WM_MOUSEMOVE)
                {
                    var pt = new NativeMethods.POINT();
                    NativeMethods.GetPhysicalCursorPos(ref pt);
                    if(doCrash)
                    {

                        using (var automation = new FlaUI.UIA3.UIA3Automation())
                        {
                            var ele = automation.FromPoint(new Point(pt.x, pt.y));
                            Console.WriteLine(pt.x + "," + pt.y + " processid: " + ele.Properties.ProcessId.Value);
                            var ProcessId2 = ele.Properties.ProcessId.ValueOrDefault;
                            var Name2 = ele.Properties.Name.ValueOrDefault;
                            var ClassName2 = ele.Properties.ClassName.ValueOrDefault;
                            var Type2 = ele.Properties.ControlType.ValueOrDefault.ToString();
                            var FrameworkId2 = ele.Properties.FrameworkId.ValueOrDefault;
                        }
                        //AutomationElement element = AutomationElement.FromPoint(new System.Windows.Point(pt.x, pt.y));
                        //Console.WriteLine(pt.x + "," + pt.y + " processid: " + element.Current.ProcessId);
                        //var ProcessId = element.Current.ProcessId;
                        //var Name = element.Current.Name;
                        //var ClassName = element.Current.ClassName;
                        //var Type = element.Current.ControlType.ToString();
                        //var FrameworkId = element.Current.FrameworkId.ToString();


                    }
                    else
                    {
                        // Console.WriteLine(pt.x + "," + pt.y);
                        AutomationElement element = AutomationElement.FromPoint(new System.Windows.Point(pt.x, pt.y));
                        Console.WriteLine(pt.x + "," + pt.y + " processid: " + element.Current.ProcessId 
                            + " " + element.Current.ClassName.ToString() + " " + element.Current.ControlType.ToString());

                        using (var automation = new FlaUI.UIA3.UIA3Automation())
                        {
                            var ele = automation.FromPoint(new Point(pt.x, pt.y));
                            Console.WriteLine(pt.x + "," + pt.y + " processid: " + ele.Properties.ProcessId.ValueOrDefault
                                + " " + ele.Properties.ClassName.ValueOrDefault?.ToString() + " " + ele.Properties.ControlType.ValueOrDefault.ToString());
                        }
                    }

                }
                return NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return IntPtr.Zero;
            }
        }
        static void Main(string[] args)
        {
            bool haderror = false;
            try
            {
                mouseProc = LowLevelMouseProc;
                mouseHook = NativeMethods.SetWindowsHookEx(NativeMethods.WH_MOUSE_LL, mouseProc, IntPtr.Zero, 0);
                Console.WriteLine("Press ctrl+C to exit");
                NativeMethods.MSG msg;
                while ((!NativeMethods.GetMessage(out msg, IntPtr.Zero, 0, 0)))
                {
                    NativeMethods.TranslateMessage(ref msg);
                    NativeMethods.DispatchMessage(ref msg);
                }

            }
            catch (Exception ex)
            {
                haderror = true;
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                NativeMethods.UnhookWindowsHookEx(mouseHook);
            }
            Console.WriteLine("Done");
            if(haderror) Console.ReadKey();

        }
    }
}
