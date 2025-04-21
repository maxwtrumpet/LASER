MoveWithEase = {

    ease_factor = 0.01,
    destination = Vector2(0,0),

    OnStart = function (self)
        self.rb2d = self.actor:GetComponent("Rigidbody2D")
    end,

    OnUpdate = function (self)
        local cur_pos = self.rb2d:GetPosition()
        if cur_pos.x ~= self.destination.x or cur_pos.y ~= self.destination.y then
            self.rb2d:SetPosition(Vector2(cur_pos.x + (self.destination.x - cur_pos.x) * self.ease_factor,
                                          cur_pos.y + (self.destination.y - cur_pos.y) * self.ease_factor))
        end
    end

}