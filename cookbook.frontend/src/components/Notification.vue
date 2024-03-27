<template>
  <div v-if="notification.active">
    <v-snackbar
      v-model="notification.active"
      :color="notification.color"
      elevation="24"
      variant="outlined"
      :timeout="notification.timeout"
    >
      <v-icon class="ma-1">
        {{ notification.icon }}
      </v-icon>
      {{ notification.message }}
    </v-snackbar>
  </div>
</template>

<script>
export default {
  name: "Notification",
  props: ["notification", "toggleNotification"],
};
</script>
