var moment = require('moment');

module.exports = function (date, format) {
      if (!date) {
        return 'N/A'
      }
      return moment(date, 'YYYY-MM-DD HH:mm:ss').format(format);
};

// Vue.filter('moment', function (date, format) {
//     if (!date) {
//       return 'N/A'
//     }
//     return moment(date, 'YYYY-MM-DD').format(format);
// });