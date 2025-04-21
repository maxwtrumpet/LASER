GnatEnemy = {

    velocity = Vector2(0,0),
    cur_frame = 0,
    gravity_scale = 0.0166666,

    OnEnable = function (self)
        self.rb2d:SetVelocity(self.velocity)
    end,

    OnDisable = function (self)
        self.rb2d:SetVelocity(Vector2(0,0))
    end,

    OnStart = function (self)
        self.sr = self.actor:GetComponent("SpriteRenderer")
        self.rb2d = self.actor:GetComponent("Rigidbody2D")
        local angle = math.random() * 2 * math.pi
        self.velocity = Vector2(math.cos(angle) * 2, math.sin(angle) * 2)
        self.rb2d:SetVelocity(self.velocity)
    end,

    OnUpdate = function (self)
        self.velocity = self.rb2d:GetVelocity()
        local position = self.rb2d:GetPosition()
        if position.y < 0 then
            position.y = position.x
            position.x = 0
            self.rb2d:SetPosition(position)
            self.rb2d:SetVelocity(Vector2(-self.velocity.y,self.velocity.x))
        elseif position.x < 0 then
            position.x = position.y
            position.y = 0
            self.rb2d:SetPosition(position)
            self.rb2d:SetVelocity(Vector2(self.velocity.y,-self.velocity.x))
        end
        local angle = math.abs(math.atan(position.y,position.x))
        local x_sign = 1
        if position.x < 0 then
            x_sign = -1
        end
        local y_sign = 1
        if position.y < 0 then
            y_sign = -1
        end
        local gravity = Vector2(x_sign*math.cos(angle)*-self.gravity_scale,y_sign*math.cos(angle)*-self.gravity_scale)
        self.rb2d:AddForce(gravity)
    end,

    OnLateUpdate = function (self)
        self.cur_frame = self.cur_frame + 1
        if self.cur_frame % 30 == 0 then
            self.sr.sprite = "enemies/gnat/gnat_" .. tostring(math.random(0,3))
        end
    end

}