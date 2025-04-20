HealthManager = {

    cur_beam = nil,
    health = 4,
    points = 1,
    explosion = "0",
    boss = false,
    countdown = -1,
    damage0 = {},
    damage1 = {},
    damage2 = {},
    damage3 = {},

    OnStart = function (self)
        self.cur_health = self.health
        self.am = self.actor:GetComponent("Animator")
        self.am.sprite = self.actor:GetComponent("SpriteRenderer")
        self.am.frames = self.damage0
    end,

    OnUpdate = function (self)
        if self.countdown ~= -1 then
            self.countdown = self.countdown - 1
            if self.countdown == 0 then
                Actor.Destroy(self.actor)
            end
        end
    end,

    OnLateUpdate = function (self)
        if self.cur_health <= 0 and self.countdown == -1 then
            local exp = Actor.Instantiate("Explosion" .. self.explosion):GetComponent("SpriteRenderer")
            local position = self.actor:GetComponent("Rigidbody2D"):GetPosition()
            exp.x_position = position.x
            exp.y_position = position.y
            if self.points == 1000 then
                exp = exp.actor:AddComponent("BossExplosion")
                exp.iteration = 0
                exp.actor:GetComponent("BossEnemy").enabled = false
                exp.actor:GetComponent("Rigidbody2D"):SetVelocity(Vector2(0,0))
                exp.actor:GetComponent("Animator").enabled = false
                exp.actor:GetComponent("SpriteRenderer").sprite = "enemies/boss/boss_0_3"
            else
                Actor.Destroy(self.actor)
            end
            Event.Publish("Explosion", {size = self.explosion})
            Event.Publish("Kill", {points = self.points, bonus = self.health > 1})
        elseif self.cur_health <= self.health / 4 then
            self.am.frames = self.damage3
        elseif self.cur_health <= self.health / 2 then
            self.am.frames = self.damage2
        elseif self.cur_health <= 3 * self.health / 4 then
            self.am.frames = self.damage1
        end
    end,

    OnTriggerEnter2D = function(self, contact)
        if contact.other:GetName() == "Player" then
            Event.Publish("Explosion", {size = "4"})
            Event.Publish("Lose", {})
            Actor.Instantiate("ButtonMenu")
            Actor.Instantiate("ButtonRetry")
            local lose = Actor.Instantiate("GameMenu")
            lose:GetComponent("ButtonManager").button_layout = {2}
            lose:GetComponent("TextRenderer").text = "Game over..."
            Actor.Find("UI"):GetComponentByKey("Manager").enabled = false
            Actor.Find("Gun"):DisableAll()
            Actor.Find("Player"):DisableAll()
            local beam = Actor.Find("Beam")
            if beam ~= nil then
                beam:DisableAll()
            end
            local basic = Actor.FindAll("Basic")
            for index, value in ipairs(basic) do
                value:DisableAll()
            end
            local fast = Actor.FindAll("Fast")
            for index, value in ipairs(fast) do
                value:DisableAll()
            end
            local explosions = Actor.FindAll("Explosion")
            for index, value in ipairs(explosions) do
                value:DisableAll()
            end
            self.enabled = false
        elseif contact.other ~= self.cur_beam and contact.other:GetName() == "Beam" then
            self.cur_beam = contact.other
            self.cur_health = self.cur_health - self.cur_beam:GetComponent("BeamManager").damage
        end
    end,

    OnTriggerExit2D = function(self, contact)
        if contact.other == self.cur_beam then
            self.cur_beam = nil
        end
    end

}