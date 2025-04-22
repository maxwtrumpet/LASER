BossEnemy = {

    vel = Vector2(0,0),

    OnEnable = function (self)
        self.rb:SetVelocity(Vector2(self.vel.x / 2.5, self.vel.y / 2.5))
    end,

    OnDisable = function (self)
        self.rb:SetVelocity(Vector2(0,0))
    end,

    OnStart = function (self)

        local hm = self.actor:GetComponent("HealthManager")
        hm.damage0 = {"enemies/boss/boss_0_0","enemies/boss/boss_1_0","enemies/boss/boss_2_0",
                      "enemies/boss/boss_2_0","enemies/boss/boss_1_0","enemies/boss/boss_0_0"}
        hm.damage1 = {"enemies/boss/boss_0_1","enemies/boss/boss_1_1","enemies/boss/boss_2_1",
                      "enemies/boss/boss_2_1","enemies/boss/boss_1_1","enemies/boss/boss_0_1"}
        hm.damage2 = {"enemies/boss/boss_0_2","enemies/boss/boss_1_2","enemies/boss/boss_2_2",
                      "enemies/boss/boss_2_2","enemies/boss/boss_1_2","enemies/boss/boss_0_2"}
        hm.damage3 = {"enemies/boss/boss_0_3","enemies/boss/boss_1_3","enemies/boss/boss_2_3",
                      "enemies/boss/boss_2_3","enemies/boss/boss_1_3","enemies/boss/boss_0_3"}

        self.rb = self.actor:GetComponent("Rigidbody2D")
        local location = Vector2(19.5, math.random() * 7 + 1.25)

        self.rb:SetPosition(location)
        self.rb:SetRotation(math.atan(location.y,location.x)*180/math.pi)

        self.vel = Vector2.Normalize(Vector2(-location.x, 1.25 - location.y))

        self:OnEnable()
        Event.Publish("Music", {parameter = "Drum 3", value = 1})

    end,

    OnDestroy = function (self)
        if Actor.Find("Boss") == nil then
            Event.Publish("Music", {parameter = "Drum 3", value = 0})
        end
    end

}