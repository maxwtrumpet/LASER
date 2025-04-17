CutsceneManager = {

    messages = {{"The year is 30XX.","Humans have begun","using fusion as their","main power source."},
                {"Suddenly, leagues of","alien robots swarm","Earth. Space pirates!"},
                {"Having depleted their","own fossil fuels, they","scour the cosmos","looking for planets to","raid; Earth is next."},
                {"To stop them, the","world powers have","united to build a ray","gun that can destroy","the robots."},
                {"They put their trust","in you to man this","machine, and save","humanity."},
                {"This, soldier, is your","Last Assignment:","Save Earth from the","Robots..."}},
    shows = {110,92,152,131,96,101},
    cur_message = 1,
    cur_mode = 1,
    frame_goal = 0,

    OnStart = function (self)
        self.trs = self.actor:GetComponents("TextRenderer")
        local length = #(self.messages[1])
        for index, value in ipairs(self.messages[1]) do
            self.trs[index].text = value
            self.trs[index].y_position = (length - 1) / 2 + 1 - index
        end
        local played = io.open(Application.FullPath("resources/.data/.played"),"r")
        if played ~= nil then
            io.close(played)
            --Scene.Load("menu")
            --return
        end
        played = io.open(Application.FullPath("resources/.data/.played"),"w")
        io.close(played)
        local sd = Actor.Instantiate("StaticData")
        Actor.DontDestroy(sd)
        sd:GetComponent("MusicManager"):Init()
        Audio.SetEventParameter("event:/music/Theme", "Drum 1", 0);
        self.frame_goal = Application.GetFrame() + 100
    end,

    OnUpdate = function (self)
        local cur_frame = Application.GetFrame()
        if cur_frame == self.frame_goal then
            self.cur_mode = self.cur_mode + 1
            if self.cur_mode == 5 then
                self.cur_message = self.cur_message + 1
                if self.cur_message == 7 then
                    Scene.Load("menu")
                    return
                end
                self.trs[4].text = " "
                self.trs[5].text = " "
                local length = #(self.messages[self.cur_message])
                for index, value in ipairs(self.messages[self.cur_message]) do
                    self.trs[index].text = value
                    self.trs[index].y_position = (length - 1) / 2 + 1 - index
                end
                self.cur_mode = 1
                self.frame_goal = self.frame_goal + 100
            elseif self.cur_mode == 2 then
                self.frame_goal = self.frame_goal + self.shows[self.cur_message]
            elseif self.cur_mode == 3 then
                self.frame_goal = self.frame_goal + 100
            else
                self.frame_goal = self.frame_goal + 30
            end
        else
            if self.cur_mode == 1 then
                for i = 1, 5, 1 do
                    self.trs[i].a = self.trs[i].a + 2.55
                end
            elseif self.cur_mode == 3 then
                for i = 1, 5, 1 do
                    self.trs[i].a = self.trs[i].a - 2.55
                end
            end
        end
    end
}