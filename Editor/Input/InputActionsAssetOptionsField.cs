
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

using MaroonSeal.Inputs;

namespace MaroonSealEditor.Inputs
{
    public class InputActionAssetOptionsField : PopupField<string>
    {
        InputActionAsset asset;

        public InputActionAssetOptionsField(InputActionAsset _asset) => SetAsset(_asset);

        public void SetAsset(InputActionAsset _asset)
        {
            if (asset != _asset) { value = ""; }
            asset = _asset;
            if (!asset) {
                choices.Clear();
                formatSelectedValueCallback = cntx => "";
                formatListItemCallback = cntx => "";
            }
            else
            {
                choices = IInputActionHandler.GetAssetActionPaths(_asset);
                if (!choices.Contains(value)) { value = ""; }

                formatSelectedValueCallback = cntx => cntx;
                formatListItemCallback = cntx => cntx;
            }
        }
    }
}