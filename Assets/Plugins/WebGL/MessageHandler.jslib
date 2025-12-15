mergeInto(LibraryManager.library, {
  RegisterMessageHandler: function() {
    window.addEventListener('message', function(event) {
      if (event.data && event.data.type === 'USER_ID') {
        var id = event.data.id;
        SendMessage('LevelManager', 'SetUserIdFromWeb', id.toString());
      }
    });
  }
});