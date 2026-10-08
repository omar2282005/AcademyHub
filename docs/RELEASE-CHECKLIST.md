# AcademyHub — مراجعة قبل النشر

هذه Checklist عمل، وليست ادعاء بأن كل بند تم اختباره أو أن التطبيق جاهز للإنتاج.

## ١. مراجعة الوظائف على آخر نسخة

- [ ] وجود `return View(model)` داخل فرع !ModelState.IsValid في Edit POST.
- [ ] الإضافة تربط CreatedByUserId بالمستخدم الحالي.
- [ ] Edit/Delete تتحقق من الملكية في GET وPOST.
- [ ] عدم وجود عمليات حفظ أو حذف في GET.
- [ ] رفض معرّف محاضر غير موجود على السيرفر.
- [ ] إعادة خيارات المحاضرين عند فشل Validation.
- [ ] رسائل TempData تظهر بعد النجاح، والبحث ما زال يعمل.
- [ ] Home وPrivacy وروابط الحسابات وLogout POST تعمل بدون تكرار Navbar.
- [ ] منع حذف كورس عليه تسجيلات، بما يشمل التسجيل بعد فتح التأكيد وقبل إرساله.
- [ ] سلامة الحالات غير الموجودة أو غير المصرح بها.

## ٢. ملفات المستودع

- [ ] ملفات المشروع والحل إن وجد، وكل Models/Controllers/ViewModels/Repositories/Mapping/Contexts.
- [ ] Migrations وModel Snapshots لكل Context محفوظة في Git.
- [ ] ملفات Views وwwwroot الضرورية، بما يشمل مكتبات jQuery وBootstrap أو طريقة موثوقة لاستعادتها.
- [ ] seed-demo.sql موجود داخل scripts.
- [ ] README يطابق المشروع الفعلي ولا يذكر مميزات غير موجودة.
- [ ] لا ملفات bin/obj/.vs أو قواعد بيانات أو نسخ احتياطية أو Logs شخصية.
- [ ] لا Passwords أو Tokens أو Connection Strings ببيانات استضافة أو License Keys.
- [ ] مراجعة appsettings.Development.json وlaunchSettings.json إن وجدا؛ .gitignore لا يحمي المحتوى تلقائيًا.

## ٣. تجربة نسخة نظيفة بدون المساس ببياناتك

- [ ] احفظ/Commit ملفاتك أولًا.
- [ ] افتح نسخة ثانية في مجلد مختلف عن نسخة العمل.
- [ ] استخدم قاعدة تدريب جديدة مثل AcademyHubTrainingDb_Verification وقاعدة حسابات جديدة مثل AcademyHubIdentityDb_Verification في النسخة الثانية فقط.
- [ ] لا تنفّذ Drop-Database أو تحذف قواعدك الأصلية.
- [ ] Restore ثم Build.
- [ ] Update-Database -Context TrainingDbContext.
- [ ] Update-Database -Context IdentityAppDbContext.
- [ ] عدّل ExpectedDatabase في نسخة محلية من seed-demo.sql للاسم المؤقت، وشغّلها على قاعدة التحقق الجديدة.
- [ ] شغّل seed ثانية وتأكد من عدم التكرار.
- [ ] Register ثم Create/Edit/Delete بكورس الحساب الجديد.
- [ ] راجع Default LocalDB configuration قبل رفع أي تعديلات خاصة بالاختبار.

## ٤. العرض على GitHub وLinkedIn

- [ ] Screenshots بحجم طبيعي، بدون بيانات حساب شخصية أو درجات طلاب حقيقيين.
- [ ] فيديو مختصر يعرض Workflow حقيقيًا: إنشاء، تعديل، Validation، ومنع وصول غير صاحب الكورس.
- [ ] وصف المشروع بأنه مشروع Backend تعليمي بإدارة كورسات، لا منصة LMS كاملة جاهزة للإنتاج.
- [ ] رابط GitHub الحقيقي بعد إنشاء المستودع، وعدم إضافة رابط live demo قبل وجود نسخة منشورة فعلًا.
- [ ] تحديد قرار ترخيص الكود إذا رغبت؛ لم يُفترض ترخيص MIT أو غيره تلقائيًا.

## ملاحظة Git مهمة

.gitignore تمنع إضافة ملفات غير متتبعة مستقبلًا. لو ملف حساس متتبع بالفعل، مجرد إضافته إلى .gitignore لا يزيله من Git أو تاريخه. افحص git status وgit ls-files قبل الرفع. لو سبق نشر سر، ألغِه/غيّره بدل الاكتفاء بحذفه من الملف الحالي.
