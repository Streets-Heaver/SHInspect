using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Input;
using SHAutomation.Core.StaticClasses;
using SHInspect.Classes;
using SHInspect.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SHInspect.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {

        public void Start()
        {
            _dispatcherTimer.Start();
            _previousRefresh = DateTime.Now;
        }

        public void Stop()
        {
            _dispatcherTimer.Stop();
        }

        public void DrawHighlight(ISHAutomationElement element)
        {
            ElementHighlighter.UseElementHighlighter = true;
            var drawingcolor = System.Drawing.Color.FromArgb(
            SelectedColour.A, SelectedColour.R, SelectedColour.G, SelectedColour.B);
            ElementHighlighter.HighlightColour = drawingcolor;
            ElementHighlighter.HighlightElement(element as SHAutomationElement);
            ElementHighlighter.UseElementHighlighter = false;
        }

        public async Task Inspect(bool acceptNewWindows)
        {

            if (!Inspecting)
            {
                Inspecting = true;
                await Task.Run(() =>
                {
                    try
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(delegate
                        {
                            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
                            {
                                if (w is AddWindowPopup)
                                {
                                    w.Close();
                                }
                            }
                        });
                        var screenPos = Mouse.Position;
                        var hov = Automation.FromPoint(screenPos);
                        ElementBO hoveredElement = hov != null ? new ElementBO(hov) : null;
                        var win = hoveredElement != null ?  GetRootFromElement(hoveredElement) : null;
                        if(win == null)
                        {
                            Inspecting = false;
                            return;
                        }
                        if (SavedSettingsWindows.Any(x => x.Identifier == win.AutomationId || x.Identifier == win.Name))
                        {
                            if (!Elements.Any(x => x.AutomationElement.Equals(win) && x.AutomationElement.ProcessId == win.ProcessId))
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                                {
                                    GetDesktop();
                                });
                            }
                            else
                            {
                                DrawHighlight(hov);
                            }
                        }
                        else
                        {
                            Inspecting = false;
                            if (acceptNewWindows)
                            {
                                CreateAddWindowPopUp(win);
                            }
                            return;
                        }
                        if (hoveredElement != null)
                        {
                            var selected = SelectedItem;
                            if (selected == null || (selected != null && !selected.Equals(hoveredElement)))
                            {
                                SelectedItem = hoveredElement;
                                ElementToSelectChanged(hoveredElement.AutomationElement);
                                Inspecting = false;
                            }
                        };

                    }
                    catch (Exception ex) when (ex is COMException || ex is UnauthorizedAccessException || ex is Win32Exception || ex is FileNotFoundException || ex is TimeoutException)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.GetType().Name);
                        Inspecting = false;
                        System.Windows.Application.Current.Dispatcher.Invoke(delegate
                        {
                            GetDesktop();
                        });
                    }
                });
            }
        }
        private void CreateAddWindowPopUp(ISHAutomationElement window)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                AddWindowPopup pop = new AddWindowPopup(new WindowBO(window, false));
                pop.Left = Mouse.Position.X;
                pop.Top = Mouse.Position.Y;
                pop.Show();
                pop.BringIntoView();
                pop.Topmost = true;
            });
        }
        private async void DispatcherTimerTick(object sender, EventArgs e)
        {
            if (System.Windows.Input.Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Shift) && System.Windows.Input.Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Control) && System.Windows.Input.Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Alt))
            {
                await Inspect(true);
            }
            else if (System.Windows.Input.Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Control) && System.Windows.Input.Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Alt))
            {
                await Inspect(false);
            }
            else
            {
                if (IsLive && DateTime.Now > _previousRefresh.AddSeconds(1))
                {
                    _previousRefresh = DateTime.Now;
                    RefreshItemDetails();
                }
                else if (!IsLive)
                {
                    _previousRefresh = DateTime.Now;
                }
            }

        }
    }
}
