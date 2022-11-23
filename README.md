# Desgram
1)Первое что хочется пояснить почему я использую двойной маппинг в таких моментах как:
```C#
var posts = await _context.Users.AsNoTracking()
    .Where(u=>u.Id == model.UserId)
    .SelectMany(u => u.Posts)
    .Where(p=>p.DeletedDate == null)
    .Skip(model.Skip).Take(model.Take)
    .ProjectToByRequestorId<PostModel>(_mapper.ConfigurationProvider,requestorId)
    .ToListAsync();

return posts.Select(p=>_mapper.Map<PostModel>(p)).ToList();
```
и еще

```C#
var users = await _context.Users.AsNoTracking()
  .Where(u => u.Name.ToLower().Contains(model.SearchUserName.ToLower())
              && !u.BlockedUsers.Any(ub => ub.BlockedId == requestorId && ub.DeletedDate == null))
  .Skip(model.Skip).Take(model.Take)
  .ProjectTo<PartialUserModel>(_mapper.ConfigurationProvider)
  .ToListAsync();

return users.Select(u=>_mapper.Map<PartialUserModel>(u)).ToList();
```
Здесь сначала используется ProjectTo для проекции User в PartialUserModel, а потом mapper.Map() для PartialUserModel в PartialUserModel. Это зделано так, потому что
во многих случиях при маппинге Post в PostModel или User в UserModel и если использовать mapper.Map(), то для подчета количества, например, постов придется писать Include, что в свою очередь загрузит все посты пользователя только для подчета их количества, я посчитал что это неприятно, поэтому я использою сначала ProjectTo, чтобы достать все неоходимое малой кровью, а mapper.Map() для того чтобы в рантайме получить url для аттачей которые содержатся в моделях. Лучшего решения я не придумал.

2) Также через небольшой кастыль я смог пробрацивать в ProjectTo requestId(id пользователя чей запрос обрабатывается), а именно с помощью данной функции:
```C#
 public static IQueryable<T> ProjectToByRequestorId<T>(this IQueryable queryable, AutoMapper.IConfigurationProvider provider, Guid requestorId)
        {
            MapperProfile.RequestorId = requestorId;
            return queryable.ProjectTo<T>(provider);
        }
```
RequestorId являеся статическим полем в классе MapperProfile:
```C#
 public class MapperProfile : Profile
    {
        public static Guid RequestorId { get; set; }
       ...
       ...
       ...
```
3) В данный момент содержимым контента поста могут быть только фото, но в дальнейшем это планируется расширить видео, не добавил сейчас, из-за сложности и не понимания как нужно хранить и загружать видео(я так понимаю не одним большим файлом, а разбивать на более мелкие).
4) В интерфейсах служб есть комментарии, что делают методы
