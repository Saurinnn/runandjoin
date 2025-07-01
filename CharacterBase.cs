// CharacterBase.cs CharacterBase�R���|�[�l���g�̍쐬
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CharacterBase : MonoBehaviour
{
    [Range(0, 20)] public float SpeedPerSeconds = 5f;

    Map Map { get => Object.FindObjectOfType<Map>(); }
    public bool IsStop { get; set; } = false;
    public string DisplayName = "Character";  // 初期値

    public Vector3 Pos
    {
        get => transform.position; private set => transform.position = value;
    }

    public Vector3 Forward { get => transform.forward; }

    public void Move(Vector3 velocity)
    {
        Pos += velocity * Time.deltaTime;
        Pos = Map.ClampPos(Pos);

        transform.LookAt(Pos + velocity, Vector3.up);
    }

    protected virtual void FixedUpdate()
    {
        var rigidBody = GetComponent<Rigidbody>();
        var vec = rigidBody.velocity;
        vec.x = 0; vec.z = 0;
        rigidBody.velocity = vec;
        rigidBody.angularVelocity = Vector3.zero;
    }

    protected virtual void Awake() 
    { 
        ChangeTeamColor(_teamColor);
    }
     [SerializeField] int _memberCount = 0;
    public int MemberCount { get => _memberCount; set => _memberCount = value; }
    public bool IsJoinableCharacter = false;
    [SerializeField] Color _teamColor = Color.red;
    public Color TeamColor { get => _teamColor; }
 
    protected void ChangeTeamColor(Color color)
    {
        _teamColor = color;
        var model = transform.Find("Model");
        if (model != null)
        {
            var renderer = model.GetComponent<Renderer>();
            renderer.material.color = _teamColor;
        }
    }
 
        protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsStop) return;
        var character = other.gameObject.GetComponent<CharacterBase>();
        if (character != null && character.IsJoinableCharacter)
        {
            if (!(this is JoinableCharacter))
            {
                var joinableChar = character as JoinableCharacter;
                if (joinableChar.FollowedTarget != null) joinableChar.FollowedTarget.MemberCount--;
                joinableChar.FollowedTarget = this;
                MemberCount++;
            }
        }
    }
    
    protected virtual void Start() { }
}