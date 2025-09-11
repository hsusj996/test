using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using super_rookie.Core;
using super_rookie.Models;
using super_rookie.ViewModels.Messages;

namespace super_rookie.ViewModels
{
    public partial class MainVM : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<MixingUnitVM> _mixingUnits;

        [ObservableProperty]
        private MixingUnitVM? _selectedMixingUnit;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        public MainVM()
        {
            _mixingUnits = new ObservableCollection<MixingUnitVM>();
            _selectedMixingUnit = null;
            _isLoading = false;
            _statusMessage = "Ready";
            
            
            LoadMixingUnitsFromStore();
        }

        // MixingUnit management methods
        public void AddMixingUnit(MixingUnitVM mixingUnit)
        {
            if (mixingUnit != null)
            {
                _mixingUnits.Add(mixingUnit);
                UpdateStatusMessage($"Added mixing unit: {mixingUnit.Name}");
            }
        }

        public void AddMixingUnit(string chemId, string ipAddress, string name)
        {
            var mixingUnit = new MixingUnitVM(chemId, ipAddress, name);
            AddMixingUnit(mixingUnit);
        }

        public void RemoveMixingUnit(MixingUnitVM mixingUnit)
        {
            if (mixingUnit != null && _mixingUnits.Contains(mixingUnit))
            {
                _mixingUnits.Remove(mixingUnit);
                UpdateStatusMessage($"Removed mixing unit: {mixingUnit.Name}");
                
                // Clear selection if removed unit was selected
                if (_selectedMixingUnit == mixingUnit)
                {
                    SelectedMixingUnit = null;
                }
            }
        }

        public void RemoveMixingUnitAt(int index)
        {
            if (index >= 0 && index < _mixingUnits.Count)
            {
                var mixingUnit = _mixingUnits[index];
                RemoveMixingUnit(mixingUnit);
            }
        }

        public void ClearAllMixingUnits()
        {
            _mixingUnits.Clear();
            SelectedMixingUnit = null;
            UpdateStatusMessage("Cleared all mixing units");
        }

        // Selection management
        partial void OnSelectedMixingUnitChanged(MixingUnitVM? value)
        {
            if (value != null)
            {
                UpdateStatusMessage($"Selected mixing unit: {value.Name}");
            }
            else
            {
                UpdateStatusMessage("No mixing unit selected");
            }
        }

        // Utility methods
        public MixingUnitVM? GetMixingUnitByChemId(string chemId)
        {
            return _mixingUnits.FirstOrDefault(mu => mu.ChemId == chemId);
        }

        public MixingUnitVM? GetMixingUnitByIpAddress(string ipAddress)
        {
            return _mixingUnits.FirstOrDefault(mu => mu.IpAddress == ipAddress);
        }

        public MixingUnitVM? GetMixingUnitByName(string name)
        {
            return _mixingUnits.FirstOrDefault(mu => mu.Name == name);
        }

        public bool ContainsMixingUnit(string chemId)
        {
            return _mixingUnits.Any(mu => mu.ChemId == chemId);
        }

        public int GetMixingUnitCount()
        {
            return _mixingUnits.Count;
        }

        // Status management
        private void UpdateStatusMessage(string message)
        {
            StatusMessage = message;
        }

        public void SetLoading(bool isLoading, string? message = null)
        {
            IsLoading = isLoading;
            if (message != null)
            {
                StatusMessage = message;
            }
            else if (!isLoading)
            {
                StatusMessage = "Ready";
            }
        }

        // Load from Store
        private void LoadMixingUnitsFromStore()
        {
            var store = MixingUnitStore.Instance;
            foreach (var mixingUnit in store.MixingUnits)
            {
                var mixingUnitVM = new MixingUnitVM(mixingUnit);
                _mixingUnits.Add(mixingUnitVM);
            }
            UpdateStatusMessage($"Loaded {_mixingUnits.Count} mixing units from store");
        }

        // Data operations
        public void LoadMixingUnits(IEnumerable<MixingUnit> mixingUnits)
        {
            SetLoading(true, "Loading mixing units...");
            
            try
            {
                ClearAllMixingUnits();
                
                foreach (var mixingUnit in mixingUnits)
                {
                    var mixingUnitVM = new MixingUnitVM(mixingUnit);
                    _mixingUnits.Add(mixingUnitVM);
                }
                
                UpdateStatusMessage($"Loaded {_mixingUnits.Count} mixing units");
            }
            finally
            {
                SetLoading(false);
            }
        }

        public List<MixingUnit> GetAllMixingUnitsAsModels()
        {
            return _mixingUnits.Select(mu => mu.GetModel()).ToList();
        }

        // Statistics
        public int GetTotalTanksCount()
        {
            return _mixingUnits.Sum(mu => mu.Tanks.Count);
        }

        public int GetTotalValvesCount()
        {
            return _mixingUnits.Sum(mu => mu.Valves.Count);
        }

        public int GetTotalLevelSensorsCount()
        {
            return _mixingUnits.Sum(mu => mu.LevelSensors.Count);
        }

        public int GetTotalHeatersCount()
        {
            return _mixingUnits.Sum(mu => mu.Heaters.Count);
        }

        public int GetTotalMixersCount()
        {
            return _mixingUnits.Sum(mu => mu.Mixers.Count);
        }

        public int GetTotalPumpsCount()
        {
            return _mixingUnits.Sum(mu => mu.Pumps.Count);
        }

        public int GetTotalDigitalInputsCount()
        {
            return _mixingUnits.Sum(mu => mu.DigitalInputs.Count);
        }

        public int GetTotalDigitalOutputsCount()
        {
            return _mixingUnits.Sum(mu => mu.DigitalOutputs.Count);
        }

        public int GetTotalAnalogInputsCount()
        {
            return _mixingUnits.Sum(mu => mu.AnalogInputs.Count);
        }

        public int GetTotalFunctionsCount()
        {
            return _mixingUnits.Sum(mu => mu.Functions.Count);
        }

        // Validation
        public bool ValidateMixingUnit(MixingUnitVM mixingUnit)
        {
            if (mixingUnit == null) return false;
            
            // Check for duplicate ChemId
            var existingByChemId = _mixingUnits.FirstOrDefault(mu => mu.ChemId == mixingUnit.ChemId && mu != mixingUnit);
            if (existingByChemId != null) return false;
            
            // Check for duplicate IP Address
            var existingByIp = _mixingUnits.FirstOrDefault(mu => mu.IpAddress == mixingUnit.IpAddress && mu != mixingUnit);
            if (existingByIp != null) return false;
            
            return true;
        }

        public string GetValidationMessage(MixingUnitVM mixingUnit)
        {
            if (mixingUnit == null) return "Mixing unit is null";
            
            var existingByChemId = _mixingUnits.FirstOrDefault(mu => mu.ChemId == mixingUnit.ChemId && mu != mixingUnit);
            if (existingByChemId != null) return $"ChemId '{mixingUnit.ChemId}' already exists";
            
            var existingByIp = _mixingUnits.FirstOrDefault(mu => mu.IpAddress == mixingUnit.IpAddress && mu != mixingUnit);
            if (existingByIp != null) return $"IP Address '{mixingUnit.IpAddress}' already exists";
            
            return "Valid";
        }
    }
}
