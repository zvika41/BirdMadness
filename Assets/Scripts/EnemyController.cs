using System.Collections;
using UnityEngine;

[SelectionBase]
public class EnemyController : MonoBehaviour
{
    #region --- Serialize Fields ---

    [SerializeField] private Sprite deadSprite;
    [SerializeField] private ParticleSystem enemyParticleSystem;

    #endregion Serialize Fields


    #region --- Members ---

    private SpriteRenderer _spriteRenderer;
    private bool _hasEnemyDied;

    #endregion Members


    #region --- Mono Methods ---

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    #endregion Mono Methods
    
    
    #region --- On Collision Events Handler ---

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!ShouldDieFromCollision(collision)) return;
        
        StartCoroutine(HandleEnemyDied());
    }

    #endregion On Collision Events Handle


    #region --- Private Methods ---

    private bool ShouldDieFromCollision(Collision2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        return playerController != null || collision.contacts[0].normal.y < 0.5f && !_hasEnemyDied;
    }

    private IEnumerator HandleEnemyDied()
    {
        _hasEnemyDied = true;
       _spriteRenderer.sprite = deadSprite;
       enemyParticleSystem.Play();
       yield return new WaitForSeconds(1);
      
       gameObject.SetActive(false);
    }
    
    #endregion Private Methods
}