import { reactive, ref } from "vue";

const data = reactive({
  message: "",
  icon: "mdi-check-circle-outline",
  color: "green",
  timeout: ref(3000),
  active: false,
});

const useNotification = () => {
  const setNotification = (newNotification) => {
    data.message = newNotification.message;
    data.icon = newNotification.icon;
    data.color = newNotification.color;
    return (data.active = true);
  };
  const toggleNotification = () => {
    data.active = !data.active;
  };

  return {
    notification: data,
    setNotification,
    toggleNotification,
  };
};

export default useNotification;
