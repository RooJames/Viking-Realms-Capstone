using UnityEngine;

// Attach to an NPC GameObject. Requires a SpriteRenderer and a CircleCollider2D (trigger) for interaction range.
// Sprite sheet row order: 0 = South, 1 = West, 2 = East, 3 = North
public class NPC : MonoBehaviour, IInteractable
{
    [Header("Identity")]
    public string npcName = "Villager";

    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] dialogueLines = { "Hello, traveler.", "Safe journeys to you." };

    [Header("Sprite Sheet")]
    // Assign the sliced idle sprites for each direction:
    // [0] = South, [1] = West, [2] = East, [3] = North
    public Sprite[] idleSprites = new Sprite[4];

    [Header("Animator (optional)")]
    // If you have an Animator controller set up, assign it here.
    // The controller should have int parameters 'FaceX' and 'FaceY'.
    public Animator animator;

    // ── Direction enum matching sprite sheet row order ──────────────────────
    public enum Direction { South = 0, West = 1, East = 2, North = 3 }

    private Direction _facing = Direction.South;
    private SpriteRenderer _sr;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        SetFacing(Direction.South);
    }

    // ── IInteractable ────────────────────────────────────────────────────────

    public bool CanInteract()
    {
        if (DialogueUI.Instance == null) return true;
        return !DialogueUI.Instance.IsOpen;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        // Face the player when spoken to
        FacePlayer();

        // Always track talking to this NPC (handles TalkToNPC quest objectives)
        QuestManager.Instance?.TrackTalk(npcName);

        // Let the QuestGiver handle dialogue if one is attached
        QuestGiver questGiver = GetComponent<QuestGiver>();
        if (questGiver != null && questGiver.TryHandleInteraction())
            return;

        DialogueUI.Instance.ShowDialogue(npcName, dialogueLines);
    }

    // ── Direction helpers ────────────────────────────────────────────────────

    public void SetFacing(Direction dir)
    {
        _facing = dir;

        // Sprite-swap fallback (works without an Animator)
        if (idleSprites != null && idleSprites.Length > (int)dir && idleSprites[(int)dir] != null)
            _sr.sprite = idleSprites[(int)dir];

        // Animator path (optional)
        if (animator != null)
        {
            Vector2 animDir = DirectionToVector(dir);
            animator.SetFloat("FaceX", animDir.x);
            animator.SetFloat("FaceY", animDir.y);
        }
    }

    private void FacePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 toPlayer = player.transform.position - transform.position;
        SetFacing(VectorToDirection(toPlayer));
    }

    // Maps a 2D offset to the closest cardinal direction
    private Direction VectorToDirection(Vector2 v)
    {
        if (Mathf.Abs(v.x) >= Mathf.Abs(v.y))
            return v.x >= 0 ? Direction.East : Direction.West;
        else
            return v.y >= 0 ? Direction.North : Direction.South;
    }

    private Vector2 DirectionToVector(Direction dir)
    {
        return dir switch
        {
            Direction.South => Vector2.down,
            Direction.West  => Vector2.left,
            Direction.East  => Vector2.right,
            Direction.North => Vector2.up,
            _               => Vector2.down
        };
    }
}
