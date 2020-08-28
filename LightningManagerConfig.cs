using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch;

namespace LightningManager
{
    public class LightningManagerConfig : ViewModel
    {

        private int _LightningDamage = -1;
        private int _LightningCharacterHitIntervalMin = -1;
        private int _LightningCharacterHitIntervalMax = -1;
        private int _LightningGridHitIntervalMin = -1;
        private int _LightningGridHitIntervalMax = -1;
        private int _LightningIntervalMin = -1;
        private int _LightningIntervalMax = -1;

        public int LightningDamage { get => _LightningDamage; set => SetValue(ref _LightningDamage, value); }
        public int LightningCharacterHitIntervalMax { get => _LightningCharacterHitIntervalMax; set => SetValue(ref _LightningCharacterHitIntervalMax, value); }
        public int LightningCharacterHitIntervalMin { get => _LightningCharacterHitIntervalMin; set => SetValue(ref _LightningCharacterHitIntervalMin, value); }
        public int LightningGridHitIntervalMax { get => _LightningGridHitIntervalMax; set => SetValue(ref _LightningGridHitIntervalMax, value); }
        public int LightningGridHitIntervalMin { get => _LightningGridHitIntervalMin; set => SetValue(ref _LightningGridHitIntervalMin, value); }
        public int LightningIntervalMin { get => _LightningIntervalMin; set => SetValue(ref _LightningIntervalMin, value); }
        public int LightningIntervalMax { get => _LightningIntervalMax; set => SetValue(ref _LightningIntervalMax, value); }
    }
}
