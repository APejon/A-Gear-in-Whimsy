using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private EnemyBehaviour _scriptDisable;
    private EnemyAttack _scriptDisable2;
    private SkinnedMeshRenderer _childSkinRenderer;
    private MeshRenderer _childRenderer;
    public Material flashMaterial;
    private Material originalMaterial;
    private Rigidbody _enemyRigidBody;
    private PlayerStats _plStats;
    private Inventory _inventory;
    private UIResourceManager _UI;
    public int woodDrop;
    public int stoneDrop;
    public int crystalDrop;
    public int maxHealth = 5;
    public int currentHealth;

    private Animator _animator;


    void Start()
    {
        _scriptDisable = gameObject.GetComponent<EnemyBehaviour>();
        _scriptDisable2 = gameObject.GetComponent<EnemyAttack>();
        _childSkinRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _childRenderer = GetComponentInChildren<MeshRenderer>();
        if (_childSkinRenderer)
            originalMaterial = _childSkinRenderer.material;
        else if (_childRenderer)
            originalMaterial = _childRenderer.material;
        _enemyRigidBody = GetComponent<Rigidbody>();
        _plStats = PlayerStats.Instance;
        _inventory = Inventory.Instance;
        _UI = UIResourceManager.Instance;
        currentHealth = maxHealth;
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage, Vector3 hitDirection)
    {
        currentHealth -= damage;

        StartCoroutine(DamageFlash(hitDirection.normalized));
        if (currentHealth <= 0)
            Die(hitDirection.normalized);
    }

    IEnumerator DamageFlash(Vector3 hitDirection)
    {
        if (_childSkinRenderer)
            _childSkinRenderer.material = flashMaterial;
        else if (_childRenderer)
            _childRenderer.material = flashMaterial;
        _scriptDisable.enabled = false;
        _enemyRigidBody.AddForce((hitDirection * 30) + (Vector3.up * 30), ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        if (_childSkinRenderer)
            _childSkinRenderer.material = originalMaterial;
        else if (_childRenderer)
            _childRenderer.material = originalMaterial;
        _scriptDisable.enabled = true;
    }

    void Die(Vector3 hitDirection)
    {
        // Debug.Log("Enemy died!");

        _plStats.SetPlayerState(PlayerStats.State.Safe);
        this.enabled = false;
        _scriptDisable.enabled = false;
        _scriptDisable2.enabled = false;
        DropLoot();
        _enemyRigidBody.AddForce((hitDirection * 40) + (Vector3.up * 40), ForceMode.Impulse);
        StartCoroutine(Dissolve());
        // Destroy(gameObject, 3f); // Destroy game object after 3 seconds
    }

    public void DropLoot()
    {
        _inventory.AddWood(woodDrop);
        _inventory.AddStone(stoneDrop);
        _inventory.AddCrystal(crystalDrop);
        _UI.updateUI();
    }

    IEnumerator Dissolve()
    {
        _animator.enabled = true;
        yield return new WaitForSeconds(1.0f);
        _animator.enabled = false;
        yield return new WaitForSeconds(10.0f);
        gameObject.SetActive(false);
    }
}
