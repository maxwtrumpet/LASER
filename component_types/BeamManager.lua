BeamManager = {
    goal_thickness = 2,
    grow_time = 0.2,
    damage = 1,

    OnStart = function (self)
        self.actor:GetComponent("Rigidbody2D"):SetTriggerDimensions(Vector2(20,self.actor:GetComponent("SpriteRenderer").y_scale / 3.125 * 2))
        local am = self.actor:GetComponent("Animator")
        for i = 0, 63, 1 do
            am.frames[i+1] = "beam/beam_" .. i
        end
        am.sprite = self.actor:GetComponent("SpriteRenderer")
    end,

    OnDestroy = function (self)
        Actor.Find("UI"):GetComponentByKey("Manager").bonus_points = 0
    end
}