KamikazeEnemy = {

    vel = Vector2(0,0),
    speed_factor = 0,
    angle = 0,

    OnEnable = function (self)
        self.rb:SetVelocity(Vector2(-self.vel.x * self.speed_factor, -self.vel.y * self.speed_factor))
    end,

    OnDisable = function (self)
        self.rb:SetVelocity(Vector2(0,0))
    end,

    OnStart = function (self)

        local hm = self.actor:GetComponent("HealthManager")
        hm.damage0 = {"enemies/kamikaze/kamikaze_0","enemies/kamikaze/kamikaze_1","enemies/kamikaze/kamikaze_2",
                      "enemies/kamikaze/kamikaze_2","enemies/kamikaze/kamikaze_1","enemies/kamikaze/kamikaze_0"}
        hm.damage1 = hm.damage0
        hm.damage2 = hm.damage0
        hm.damage3 = hm.damage0

        self.rb = self.actor:GetComponent("Rigidbody2D")
        local top_or_bottom = math.random(3)

        local location = Vector2(math.random() * 17 + 1, 8.5)
        if top_or_bottom == 1 then
            location = Vector2(18, math.random() * 8.5)
        end

        self.rb:SetPosition(location)
        self.prev_location = Vector2.Length(location)
        self.angle = -math.atan(location.x,location.y)*180/math.pi
        self.rb:SetRotation(self.angle)

        self.speed_factor = (Vector2.Length(location) - 10) * 0.625 + 4
        self.vel = Vector2.Normalize(location)

        self:OnEnable()
        local smoke = Actor.Instantiate("Smoke"):GetComponent("SpriteRenderer")
        smoke.z_rotation = self.angle
        smoke.x_position = location.x
        smoke.y_position = location.y
        Event.Publish("Music", {parameter = "Ostinato Fast", value = 1})

    end,

    OnUpdate = function (self)
        local cur_location = self.rb:GetPosition()
        if self.prev_location - Vector2.Length(cur_location) >= 0.16 then
            local smoke = Actor.Instantiate("Smoke"):GetComponent("SpriteRenderer")
            smoke.z_rotation = self.angle
            smoke.x_position = cur_location.x
            smoke.y_position = cur_location.y
        end
    end,

    OnDestroy = function ()
        if Actor.Find("Kamikaze") == nil then
            Event.Publish("Music", {parameter = "Ostinato Fast", value = 0})
        end
    end

}