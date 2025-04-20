BasicEnemy = {

    vel = Vector2(0,0),
    speed_factor = 0,

    OnEnable = function (self)
        self.rb:SetVelocity(Vector2(-self.vel.x * self.speed_factor, -self.vel.y * self.speed_factor))
    end,

    OnDisable = function (self)
        self.rb:SetVelocity(Vector2(0,0))
    end,

    OnStart = function (self)

        local hm = self.actor:GetComponent("HealthManager")
        hm.damage0 = {"enemies/basic/basic_0_0","enemies/basic/basic_1_0","enemies/basic/basic_2_0",
                      "enemies/basic/basic_2_0","enemies/basic/basic_1_0","enemies/basic/basic_0_0"}
        hm.damage1 = {"enemies/basic/basic_0_1","enemies/basic/basic_1_1","enemies/basic/basic_2_1",
                      "enemies/basic/basic_2_1","enemies/basic/basic_1_1","enemies/basic/basic_0_1"}
        hm.damage2 = {"enemies/basic/basic_0_2","enemies/basic/basic_1_2","enemies/basic/basic_2_2",
                      "enemies/basic/basic_2_2","enemies/basic/basic_1_2","enemies/basic/basic_0_2"}
        hm.damage3 = {"enemies/basic/basic_0_3","enemies/basic/basic_1_3","enemies/basic/basic_2_3",
                      "enemies/basic/basic_2_3","enemies/basic/basic_1_3","enemies/basic/basic_0_3"}

        self.rb = self.actor:GetComponent("Rigidbody2D")
        local top_or_bottom = math.random(3)

        local location = Vector2(math.random() * 17 + 1, 8.5)
        if top_or_bottom == 1 then
            location = Vector2(18, math.random() * 8.5)
        end

        self.rb:SetPosition(location)
        self.rb:SetRotation(math.atan(location.y,location.x)*180/math.pi)

        self.speed_factor = Vector2.Length(location) / 10
        self.vel = Vector2.Normalize(location)

        self:OnEnable()

    end
}