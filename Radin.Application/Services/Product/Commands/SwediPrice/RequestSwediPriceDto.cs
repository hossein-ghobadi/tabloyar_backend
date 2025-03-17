using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Product.Commands.SwediPrice
{
    public class RequestSwediPriceDto
    {
        public string Title { get; set; }
        public float EdgeSize { get; set; }
        public string EdgeColor { get; set; }
        public string FirstLayerColor { get; set; }
        public string SecondLayerModel { get; set; }
        public string SecondLayerColor { get; set; }
        public int LayerCondition { get; set; }

        public string PlexiPunchModel { get; set; }
        public string EdgePunchModel { get; set; }
        public string CrystalModel { get; set; }
        //public string CrystalColor { get; set; }
        //public bool CrystalCondition { get; set; }
        public string BSmdModel { get; set; }
        public string FSmdModel { get; set; }
        public float PvcBackLightMargin { get; set; }
        public string PunchModel { get; set; }
        public string SecondPunchModel { get; set; }

        public string PowerCalculationType { get; set; }
        public Dictionary<int, int> powerdata { get; set; }


        public bool FSmdCheckpoint { get; set; }
        public bool BSmdCheckpoint { get; set; }
        public bool EdgePunchCheckpoint { get; set; }
        public bool CrystalCheckpoint { get; set; }
        public bool PowerCheckpoint { get; set; }
        public bool PvcCheckPoint { get; set; }
        public bool LayerPunchCheckpoint { get; set; }
        public bool SecondLayerPunchCheckpoint { get; set; }

        public bool PvcBackLightCheckPoint { get; set; }


    }
}
