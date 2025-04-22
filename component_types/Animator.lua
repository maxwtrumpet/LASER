Animator = {

    frames = {},
    sprite = nil,
    speed = 1,
    cur_frame = 1,

    OnUpdate = function (self)
        self.cur_frame = self.cur_frame + 0.2 * self.speed
        self.sprite.sprite = self.frames[math.floor((self.cur_frame % #self.frames) + 1)]
    end

}