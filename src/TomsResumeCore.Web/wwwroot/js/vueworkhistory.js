/*!
 * Toms Resume Website v1.0.6 (https://tomhutson.com)
 * Copyright 2023 Tom Hutson
 * Licensed under MIT (https://opensource.org/licenses/MIT)
 */

"use strict";

var vueApp = new Vue({
  el: '#vueWorkHistory',
  data: function data() {
    return {
      workHistory: null,
      loading: true,
      errored: false
    };
  },
  mounted: function mounted() {
    var _this = this;

    axios.get("/api/WorkHistory").then(function (response) {
      return _this.workHistory = response.data;
    })["catch"](function (error) {
      console.log(error);
      _this.errored = true;
    })["finally"](setTimeout(function () {
      _this.loading = false;
    }, 2000));
  }
});