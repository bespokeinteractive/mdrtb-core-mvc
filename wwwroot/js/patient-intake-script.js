jq(function() {
    $(".collapsible-header").collapsible();

    jq('#submit_form').click(function(){
        jq('form').submit();
    });

    jq('a.next-btn').click(function(){
        var index = jq(this).data('index');
        jq('.collapsible').collapsible('open', index);
    }); 

    jq('#Examination_Weight, #Examination_Height').on('change keyup blur', function(){
        var weight = jq('#Examination_Weight').val();
        var height = jq('#Examination_Height').val();
        var bmi = 0;

        if (weight > 0 && height > 0){
            bmi = (weight/height/height) * 10000;
        }

        jq('#Examination_BMI').val(bmi.toFixed(1));
    });

});