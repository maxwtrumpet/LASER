ControllerManager = {
    OnUpdate = function (self)
        if Input.NumControllersAvailable() > 0 then
            Input.ActivateControllers(1)
            self.actor:RemoveComponent(self)
        end
    end
}