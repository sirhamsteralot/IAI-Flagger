using Flagger.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flagger.Models
{
    public class FlagScreenModel
    {
        public ObservableCollection<FlagViewModel> Flags { get; set; } = new ObservableCollection<FlagViewModel>();

        public bool UpdatedFlags = false;

        public FlaggerClient FlaggerClient { get; set; }

        public ICommand ExitCommand { get; set; }

        public IDispatcherTimer? DispatcherTimer { get; set; }



        public FlagScreenModel() {
            ExitCommand = new Command(async () => {
                DispatcherTimer?.Stop();
                DispatcherTimer = null;
                await Shell.Current.GoToAsync("//" + nameof(MainPage));
            });
        }

        public async Task<bool> SyncFlags()
        {
            return await MainThread.InvokeOnMainThreadAsync(async () => {
                var flagList = new List<FlagModel>();

                if (UpdatedFlags)
                {
                    foreach(var flag in Flags)
                    {
                        flagList.Add(flag.FlagModel);
                    }
                }

                flagList = await FlaggerClient.SyncFlagStatus(flagList);
                if (flagList == null)
                    return false;

                Flags.Clear();
                UpdatedFlags = false;
                foreach (var flag in flagList)
                {
                    FlagViewModel viewModel = new FlagViewModel 
                    { 
                        FlagModel = flag, 
                        CurrentColour = flag.CurrentFlag == FlagModel.FlagType.Green ? Colors.Green : Colors.Orange 
                    };

                    viewModel.FlagClicked = new Command((object model) => {
                        var modelCast = (FlagModel)model;

                        UpdatedFlags = true;

                        switch (modelCast.CurrentFlag)
                        {
                            case FlagModel.FlagType.Green:
                                modelCast.CurrentFlag = FlagModel.FlagType.Yellow;
                                break;
                            case FlagModel.FlagType.Yellow:
                                modelCast.CurrentFlag = FlagModel.FlagType.Green;
                                break;
                            default:
                                modelCast.CurrentFlag = FlagModel.FlagType.Yellow;
                                break;
                        }
                    });
                    Flags.Add(viewModel);
  
                }

                return true;
            });
        }
    }
}
