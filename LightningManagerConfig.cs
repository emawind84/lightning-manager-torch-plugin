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

        private int _LightningDamage = 80;
        
        public int LightningDamage { get => _LightningDamage; set => SetValue(ref _LightningDamage, value); }

    }
}
