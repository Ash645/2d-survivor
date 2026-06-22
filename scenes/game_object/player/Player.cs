using Godot;
using Godot.Collections;

public partial class Player : CharacterBody2D
{
    const int maxSpeed = 125; //This sets the maximum speed of the player.
    const int accelerationSmoothing = 20; //This sets the smoothing factor for acceleration, which controls how quickly the player reaches maximum speed. A higher value will make the player accelerate faster, while a lower value will make it accelerate more slowly.
    public int totalCollidingBodies = 0;

    public HealthComponent healthComponent;
    public Timer damageintervalTimer;
    public ProgressBar healthBar;
    public Node abilities;
    public AnimationPlayer playerAnimation;
    public Node2D visuals;

    public override void _Ready()
    {
        var enemyCollision = GetNode("CollisionArea2D") as Area2D;
        abilities = GetNode("Abilities");
        healthComponent = GetNode("HealthComponent") as HealthComponent;
        damageintervalTimer = GetNode("%DamageIntervalTimer") as Timer;
        healthBar = GetNode("%PlayerHealthBar") as ProgressBar;
        healthComponent.HealthChanged += OnHealthChanged;
        var gameEvents = GetNode<GameEvents>("/root/GameEvents");
        gameEvents.AbilityUpgradeAdded += OnAbilityUpgradeAdded;
        playerAnimation = GetNode("AnimationPlayer") as AnimationPlayer;
        visuals = GetNode("Visuals") as Node2D;
        UpdateHealthDisplay();

        //signals
        damageintervalTimer.Timeout += OnDamageIntervalTimerTimeout;
        enemyCollision.BodyEntered += OnBodyEntered;
        enemyCollision.BodyExited += OnBodyExited;
    }

    public override void _Process(double delta)
    {
        var movement_vector = GetMovementVector().Normalized(); //This gets the movement vector from the input.
        var target_velocity = movement_vector * maxSpeed; //This sets the velocity of the player to the movement vector multiplied by the maximum speed.
        Velocity = Velocity.Lerp(target_velocity, (float)(1.0f - System.Math.Exp(-delta * accelerationSmoothing))); //This smoothly interpolates the player's velocity towards the target velocity using an exponential function to create a smooth acceleration effect.
        MoveAndSlide();

        if (movement_vector.X != 0 || movement_vector.Y != 0)
        {
            playerAnimation.Play("walk");
        }
        else
        {
            playerAnimation.Play("RESET");
        }
        var moveSign = Mathf.Sign(movement_vector.X);
        if (moveSign == 0)
        {
            visuals.Scale = Vector2.One;
        }
        else
        {
            visuals.Scale = new Godot.Vector2(moveSign, 1);
        }
    }

    private Vector2 GetMovementVector()
    {
        //var movement_vector = Vector2.Zero; //This sets the movement vector to zero, so that the player doesn't move if no input is detected.
        var x_movement = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"); //This gets the strength of the "move_right" and "move_left" actions, and subtracts them to get the horizontal movement.
        var y_movement = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up"); //This gets the strength of the "move_down" and "move_up" actions, and subtracts them to get the vertical movement.
        return new Vector2(x_movement, y_movement).Normalized(); //This normalizes the movement vector, so that the player moves at the same speed in all directions.
    }

    public void CheckDealDamage()
    {
        if (totalCollidingBodies == 0 ||
                !damageintervalTimer.IsStopped())
            return;

        healthComponent.Damage(1);
        damageintervalTimer.Start();
        GD.Print(healthComponent.currentHealth);
    }

    public void UpdateHealthDisplay()
    {
        healthBar.Value = healthComponent.GetHealthPercent();
    }
    public void OnBodyEntered(Node2D _otherbody)
    {
        totalCollidingBodies += 1;
        CheckDealDamage();
    }
    public void OnBodyExited(Node2D _otherbody)
    {
        totalCollidingBodies -= 1;
    }

    public void OnDamageIntervalTimerTimeout()
    {
        CheckDealDamage();
    }

    public void OnHealthChanged()
    {
        UpdateHealthDisplay();
    }

    public void OnAbilityUpgradeAdded(AbilityUpgrade _abilityUpgrade, Dictionary _currentUpgrades)
    {
        if (_abilityUpgrade is not Ability)
            return;
        var ability = _abilityUpgrade as Ability;
        abilities.AddChild(ability.AbilityControllerScene.Instantiate());
    }
}
