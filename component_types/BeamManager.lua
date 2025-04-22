BeamManager = {
    goal_thickness = 2,
    grow_time = 12,
    remaining_time = 12,
    damage = 1,
    up = true,
    identifier = 1,

    OnStart = function (self)
        self.sr = self.actor:GetComponent("SpriteRenderer")
        self.rb2d = self.actor:GetComponent("Rigidbody2D")
        self.angle = self.rb2d:GetRotation()*math.pi/180
        local am = self.actor:GetComponent("Animator")
        for i = 0, 63, 1 do
            am.frames[i+1] = "beam/beam_" .. i
        end
        am.sprite = self.actor:GetComponent("SpriteRenderer")
    end,

    OnUpdate = function (self)
        if self.sr.x_end < 1 and self.up == true then
            self.remaining_time = self.remaining_time - 1
            local fraction = (self.grow_time - self.remaining_time) / self.grow_time
            self.sr.x_end = fraction
            self.sr.x_alignment = fraction/2
            self.rb2d:SetTriggerDimensions(Vector2(20*fraction,self.sr.y_scale / 3.125 * 2))
            self.rb2d:SetPosition(Vector2(math.cos(self.angle)*(fraction*10+2.25),math.sin(self.angle)*(fraction*10+2.25)))
        elseif self.up == true then
            self.up = false
        else
            self.remaining_time = self.remaining_time + 1
            if self.remaining_time == self.grow_time then
                Actor.Find("UI"):GetComponentByKey("Manager").bonus_points = 0
                Actor.Destroy(self.actor)
            else
                local fraction = (self.grow_time - self.remaining_time) / self.grow_time
                self.sr.x_begin = 1 - fraction
                self.sr.x_alignment = fraction/2
                self.rb2d:SetTriggerDimensions(Vector2(20*fraction,self.sr.y_scale / 3.125 * 2))
                self.rb2d:SetPosition(Vector2(math.cos(self.angle)*(22.25-fraction*10),math.sin(self.angle)*(22.25-fraction*10)))
            end
        end
    end

}