using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class LayoutSelector : MonoBehaviour
{
    [SerializeField] Dropdown _dropdown;
    List<GameConfig.LayoutPreset> _presets;
    public event Action<int, int, int> OnLayoutSelected;

    public void Initialize(List<GameConfig.LayoutPreset> presets, int defaultIndex)
    {
        _presets = presets;
        _dropdown.options.Clear();

        for (int i = 0; i < _presets.Count; i++)
            _dropdown.options.Add(new Dropdown.OptionData(_presets[i].name));

        _dropdown.value = Mathf.Clamp(defaultIndex, 0, _presets.Count - 1);
        _dropdown.RefreshShownValue();

        _dropdown.onValueChanged.AddListener(idx =>
            OnLayoutSelected?.Invoke(
                _presets[idx].rows,
                _presets[idx].cols,
                idx
            )
        );

        var d = _presets[_dropdown.value];
        OnLayoutSelected?.Invoke(d.rows, d.cols, _dropdown.value);
    }
}