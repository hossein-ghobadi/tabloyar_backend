using static Radin.Application.Services.Product.Commands.ChallPrice.ChallPriceService;

namespace Endpoint.Site.Models.NestingViewModel.ChannelliumViewModel
{
    public class ChannelliumMapper
    {
        public RequestChallCostDto Mapper(ChannelliumViewModel model)
        {
            try
            {
                // Validate required properties
                if (!IsValidModel(model))
                {
                    return null; // Return null if any required field is missing
                }

                var id = model.boardType.id;
                string secondLayer = null;
                var firstLayerColor = model.data.modelLayerLetters.one.colorPelekcy.label;

                int checkpoint = model.data.modelLayerLetters.value.id;
                var punchModel = GetPunchModel(model, checkpoint, out var punchCheckpoint, out var secondPunchCheckpoint, out var secondPunchModel);

                if (checkpoint == 2)
                {
                    secondLayer = model.data.modelLayerLetters.two.layerMaterial.value.label;
                    firstLayerColor = model.data.modelLayerLetters.two.externalColorPelekcy.label;
                }

                var request = new RequestChallCostDto
                {
                    Title = model.boardType.label,
                    EdgeSize = Convert.ToSingle(model.data.edgesSize.label),
                    EdgeColor = model.data.edgeColor.label,
                    PvcCheckPoint = model.data.needPVC.value,
                    FirstLayerColor = firstLayerColor,
                    PlexiPunchModel = model.data.modelLayerLetters.one.needPunchPelekcy.nature.label,
                    CrystalModel = model.data.needCrystal.color.label,
                    BSmdModel = model.data.needPVC.backLight.nature.label,
                    FSmdModel = model.data.needPVC.frontLight.nature.label,
                    PvcBackLightMargin = Convert.ToSingle(model.data.PVCHasBackLight.margin.label),
                    PunchModel = punchModel,
                    SecondPunchModel = secondPunchModel,
                    PowerCheckpoint = model.data.power.value,
                    powerdata = model.data.power.data,
                    PowerCalculationType = model.data.power.count.label,
                    SecondLayerColor = model.data.modelLayerLetters.two.layerMaterial.color.label,
                    SecondLayerModel = model.data.modelLayerLetters.two.layerMaterial.value.label,
                    EdgePunchModel = model.data.isPunch.nature.label,
                    LayerCondition = checkpoint,
                    LayerPunchCheckpoint = punchCheckpoint,
                    SecondLayerPunchCheckpoint = secondPunchCheckpoint,
                    EdgePunchCheckpoint = model.data.isPunch.value,
                    FSmdCheckpoint = model.data.needPVC.frontLight.value,
                    BSmdCheckpoint = model.data.needPVC.backLight.value,
                    CrystalCheckpoint = model.data.needCrystal.value,
                    PvcBackLightCheckPoint = model.data.PVCHasBackLight.value
                };

                return request;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Helper methods to validate only the necessary properties
        private bool IsValidModel(ChannelliumViewModel model) =>
            model != null &&
            IsValidBoardType(model.boardType) &&
            IsValidData(model.data);

        private bool IsValidBoardType(Type boardType) =>
            boardType != null &&
            boardType.id != null &&
            !string.IsNullOrEmpty(boardType.label);

        private bool IsValidData(data data) =>
            data != null &&
            IsValidModelLayerLetters(data.modelLayerLetters) &&
            IsValidEdgeData(data) &&
            IsValidPowerData(data) &&
            IsValidPVCAndCrystal(data);

        private bool IsValidModelLayerLetters(modelLayerLetters letters) =>
            letters?.one?.colorPelekcy != null &&
            letters.one.needPunchPelekcy?.nature != null &&
            !string.IsNullOrEmpty(letters.one.needPunchPelekcy.nature.label) &&
            letters.value != null &&
            letters.value.id != null &&
            letters.two?.layerMaterial?.value != null &&
            letters.two.layerMaterial.color != null &&
            letters.two.externalColorPelekcy != null &&
            letters.two.needPunch != null &&
            letters.two.needPunchInternal != null;

        private bool IsValidEdgeData(data data) =>
            data.edgesSize != null && !string.IsNullOrEmpty(data.edgesSize.label) &&
            data.edgeColor != null && !string.IsNullOrEmpty(data.edgeColor.label);

        private bool IsValidPowerData(data data) =>
            data.power.value != null;
            //data.power != null && data.power.data != null &&
            //data.power.count != null && !string.IsNullOrEmpty(data.power.count.label);

        private bool IsValidPVCAndCrystal(data data) =>
            data.needPVC != null && data.needPVC.backLight != null && data.needPVC.frontLight != null &&
            data.needCrystal != null && data.needCrystal.color != null &&
            data.PVCHasBackLight != null && data.PVCHasBackLight.margin != null &&
            data.isPunch != null && data.isPunch.nature != null &&
            !string.IsNullOrEmpty(data.isPunch.nature.label);

        private string GetPunchModel(ChannelliumViewModel model, int checkpoint, out bool punchCheckpoint, out bool secondPunchCheckpoint, out string secondPunchModel)
        {
            punchCheckpoint = false;
            secondPunchCheckpoint = false;
            secondPunchModel = null;

            if (checkpoint == 1)
            {
                punchCheckpoint = model.data.modelLayerLetters.one.needPunchPelekcy.value;
                return model.data.modelLayerLetters.one.needPunchPelekcy.nature.label;
            }
            else if (checkpoint == 2)
            {
                punchCheckpoint = model.data.modelLayerLetters.two.needPunch.value;
                secondPunchCheckpoint = model.data.modelLayerLetters.two.needPunchInternal.value;
                secondPunchModel = model.data.modelLayerLetters.two.needPunchInternal.nature.label;
                return model.data.modelLayerLetters.two.needPunch.nature.label;
            }
            return null;
        }
    
    }
}
