using UnityEngine;
using UnityEngine.UIElements;

public class SpiritShardButton : VisualElement
{
    public string shardName;
    private Image icon;

    public new class UxmlFactory : UxmlFactory<SpiritShardButton, UxmlTraits> {}

    public SpiritShardButton()
    {
        AddToClassList("shard-button");

        // Crear imagen interna
        icon = new Image();
        icon.AddToClassList("shard-icon");
        Add(icon);

        // Eventos
        RegisterCallback<MouseEnterEvent>(OnHover);
        RegisterCallback<MouseLeaveEvent>(OnExit);
        RegisterCallback<ClickEvent>(OnClick);
    }

    public void Init(string name, Sprite sprite)
    {
        shardName = name;
        icon.sprite = sprite;
    }

    void OnHover(MouseEnterEvent evt)
    {
        AddToClassList("hover");
    }

    void OnExit(MouseLeaveEvent evt)
    {
        RemoveFromClassList("hover");
    }

    void OnClick(ClickEvent evt)
    {
        Debug.Log("Seleccionado shard: " + shardName);
    }
}