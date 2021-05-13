using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldData
{
    public Field[,] Fields;
    public Field PrevField;
    public List<Field> PathField;
    public Vector2 FieldSize;
    public Character ActiveCharacter;
    public Skill ActiveSkill;
}

public class Field : MonoBehaviour, IPointerClickHandler
{
    [Serializable]
    struct SpritePair
    {
        public FieldType fieldType;
        public Sprite sprite;
    }

    public delegate void FieldClicked(int x, int y, PointerEventData.InputButton inputButton, bool isAi);
    public event FieldClicked Notify;

    public int x;
    public int y;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private List<SpritePair> _sprites;
    [NonSerialized] private UiManagerGame uiManager;
    
    private Dictionary<FieldType, Sprite> sprites = new Dictionary<FieldType, Sprite>();

    public FieldType type
    {
        get { return _character == null ? _type : FieldType.OBSTACLE; }
        set
        {
            _type = value;
            spriteRenderer.sprite = sprites[_type];
        }
    }

    public Character character
    {
        get { return _character; }
        set
        {
            _character = value; 
        }
    }

    private FieldType _type;
    private Character _character;

    public void Awake()
    {
        uiManager = GameObject.FindWithTag("UiManager").GetComponent<UiManagerGame>();
        foreach (SpritePair spritePair in _sprites)
        {
            sprites[spritePair.fieldType] = spritePair.sprite;
        }
    }

    public void Start()
    {
        spriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y);
    }

    public void OnAI()
    {
        Notify?.Invoke(x, y, PointerEventData.InputButton.Left, true);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (character != null && eventData.button == PointerEventData.InputButton.Right)
        {
            uiManager.dataStatsPanel.characterStats = character.stats;
            uiManager.ShowPanel(UiPanelNames.StatsPanel);
        }
        Notify?.Invoke(x, y, eventData.button, false);
    }

}
